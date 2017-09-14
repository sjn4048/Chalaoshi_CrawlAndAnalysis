using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;
using System.Net;
using Abot.Core;
using Abot.Crawler;
using Abot.Poco;
using CsQuery.HtmlParser;
using HtmlParserSharp;
using HtmlAgilityPack;

namespace WebCrawler_WinForm_
{
    public static class Chalaoshi
    {
        public const int MaximumTeacherPage = 3743;
        public static Regex TeacherDataRegex = new Regex("^https://chalaoshi.cn/teacher/\\d+/");
    }

    public class TeacherData
    {
        public int id;//统一辨识号
        public string name;
        public string url;
        public string faculty;
        public bool hasEnoughData = true;
        public double score;
        public double callNameRate;

        public enum CallName_enum
        {
            No = 0,
            Possible = 1,
            Yes = 2,
            Unknown = 3,
        };
        public CallName_enum callNameState;
        public int voteNum;
        public int commentNum;
        public List<CourseData> courseList = new List<CourseData>();
        public CourseData templateCourse;

        public static List<TeacherData> totalTeacherList = new List<TeacherData>();

        public string ToString(string format)
        {
            switch (format)
            {
                case ("Score"):
                    if (hasEnoughData)
                        return score.ToString("f2");
                    else
                        return "N/A";
                case ("VoteNum"):
                    if (hasEnoughData)
                        return voteNum.ToString();
                    else
                        return "<5";
                case ("CommentNum"):
                    if (this.hasEnoughData)
                        return commentNum.ToString();
                    else
                        return "<5";
                case ("HotNum"):
                    if (hasEnoughData)
                        return commentNum + voteNum.ToString();
                    else
                        return "<10";
                case ("CallNameRate"):
                    if (hasEnoughData)
                    {
                        string callNameRate_string = (callNameRate * 100).ToString() + "%";
                        return callNameRate_string;
                    }
                    else
                        return "N/A";
                default:
                    throw new Exception("Undefined Behavior");
            }
        }
    }

    class GetData
    {
        public void GetTeacherDataFromCsv(FileStream file)
        {
            StreamReader streamReader = new StreamReader(file,Encoding. Default);
            string teacherLine = string.Empty;
            while ((teacherLine = streamReader.ReadLine()) != null)
            {
                var Line = teacherLine.Split(',');
                TeacherData thisTeacher = new TeacherData()
                {
                    name = Line[0],
                    id = int.Parse(Line[1]),
                    url = Line[2],
                    faculty = Line[3],
                };
                thisTeacher.score = (Line[4] == "N/A") ? 0 : double.Parse(Line[4]);
                thisTeacher.callNameRate = double.Parse(Line[5].Replace("%", "")) / 100.0;
                thisTeacher.voteNum = (Line[6] == "<5") ? 0 : int.Parse(Line[6]);
                thisTeacher.commentNum = (Line[7] == "<5") ? 0 : int.Parse(Line[7]);
                if (thisTeacher.voteNum < 5)
                {
                    thisTeacher.hasEnoughData = false;
                    thisTeacher.callNameState = TeacherData.CallName_enum.Unknown;
                }   
                else if (thisTeacher.callNameRate > 0.5)
                    thisTeacher.callNameState = TeacherData.CallName_enum.Yes;
                else if (thisTeacher.callNameRate > 0.2)
                    thisTeacher.callNameState = TeacherData.CallName_enum.Possible;
                else
                    thisTeacher.callNameState = TeacherData.CallName_enum.No;

                var hisCourseList = new List<CourseData>();
                double overallGPA_number;
                for (int i = 8; i < Line.Length - 2; i++)
                {
                    string courseName;
                    if ((courseName = Line[i]) == string.Empty)
                    {
                        break;
                    }
                    while (true)
                    {
                        i++;
                        try
                        {
                            overallGPA_number = double.Parse(Line[i]);
                            break;
                        }
                        catch
                        {
                            courseName += $",{Line[i]}";
                        }
                    }
                    i++;
                    string overallGPASize_string = Line[i];
                    int overallGPASize_int = int.Parse(overallGPASize_string.Replace("+", ""));
                    var thisCourseData = new CourseData()
                    {
                        CourseName = courseName,
                        GPASampleSizeOfTeacher_string = overallGPASize_string,
                        GPASampleSizeOfTeacher_int = overallGPASize_int,
                        OverallGPAOfTeacher = overallGPA_number
                    };
                    hisCourseList.Add(thisCourseData);
                    var query = from c in CourseData.courseDataList//需要debug
                                where c.CourseName == courseName
                                select c;

                    if (!query.Any())
                    {
                        var courseData = new CourseData();
                        courseData.CourseName = courseName;
                        courseData.CourseTeachers = new List<TeacherData>();
                        courseData.CourseTeachers.Add(thisTeacher);
                        courseData.TotalGPA += overallGPA_number * overallGPASize_int;
                        courseData.TotalSampleSize += overallGPASize_int;
                        courseData.OverallGPAOfThisCourse = courseData.TotalGPA / courseData.TotalSampleSize;
                        CourseData.courseDataList.Add(courseData);
                    }
                    else
                    {
                        foreach (var courseData in CourseData.courseDataList.Select(x => x).Where(x => x.CourseName == courseName))
                        {
                            courseData.CourseTeachers.Add(thisTeacher);
                            courseData.TotalGPA += overallGPA_number * overallGPASize_int;
                            courseData.TotalSampleSize += overallGPASize_int;
                            courseData.OverallGPAOfThisCourse = courseData.TotalGPA / courseData.TotalSampleSize;
                        }
                    }
                }
                thisTeacher.courseList = hisCourseList;
                TeacherData.totalTeacherList.Add(thisTeacher);
                }
            }
        }

    public class CourseData
    {
        public string CourseName;
        public List<TeacherData> CourseTeachers;
        public double OverallGPAOfThisCourse;//所有老师的总GPA
        public int TotalSampleSize;//所有老师的总样本量
        public double TotalGPA;//中间量
        public double OverallGPAOfTeacher;//某个老师的GPA
        public int GPASampleSizeOfTeacher_int;//某个老师的总样本量
        public string GPASampleSizeOfTeacher_string;
        public bool hasEnoughData = true;

        public static List<CourseData> courseDataList = new List<CourseData>();
    }

    public class SearchAlgorithm
    {
        public List<TeacherData> SearchTeacherName(string keyword, int maxResults, bool showUnrated)
        {
            if (showUnrated)
            {
                return TeacherData.totalTeacherList.Where(t => t.name.ToUpper().Contains(keyword.ToUpper())).Take(maxResults).ToList();
            }
            else
            {
                return TeacherData.totalTeacherList.Where(t => t.hasEnoughData).Where(t => t.name.ToUpper().Contains(keyword.ToUpper())).Take(maxResults).ToList();
            }
        }

        public List<CourseData> SearchCourseName(string keyword, int maxResults, bool showUnrated)
        {
            if (showUnrated)
            {
                return CourseData.courseDataList.Where(c => c.CourseName.ToUpper().Contains(keyword.ToUpper())).Take(maxResults).ToList();
            }
            else
            {
                return CourseData.courseDataList.Where(c => c.hasEnoughData).Where(c => c.CourseName.ToUpper().Contains(keyword.ToUpper())).Take(maxResults).ToList();
            }
        }
    }
}