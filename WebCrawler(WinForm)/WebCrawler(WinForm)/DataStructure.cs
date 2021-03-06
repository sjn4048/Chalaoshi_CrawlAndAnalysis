﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;
using System.Net;
using Newtonsoft.Json;

namespace WebCrawler_WinForm_
{
    public static class Chalaoshi
    {
        public const int MaximumTeacherPage = 5215;
        public static Regex TeacherDataRegex = new Regex("^https://chalaoshi.cn/teacher/\\d+/");
    }

    public class TeacherData
    {
        //爬虫直接获取的数据
        public int ID;//统一辨识号
        public string Name;
        public string Url;
        public FacultyData Faculty;
        public double Score;
        public double CallNameRate;
        public int VoteNum;
        public int CommentNum;
        public List<CourseData> CourseList = new List<CourseData>();
        public List<CommentData> CommentList = new List<CommentData>();

        //基于自然语言处理算出的数据
        public enum CallName_enum
        {
            No = 0,
            Possible = 1,
            Yes = 2,
            Unknown = 3,
        };
        public CallName_enum CallNameState;
        public struct Statistic
        {
            double Point;
            double Credit;
        };
        public Statistic TeachingSkill;  //课讲的好不好
        public Statistic HomeworkAmount; //作业（事情）多不多
        public Statistic GiveScore;      //给分好不好
        public Statistic Characteristic; //性格、处事方式
        public Statistic OverallEval;    //学生给的总体评价

        public Statistic OverallScore;   //TODO：根据用户输入的偏好进行的个性化打分

        //用于排序、显示、分析的辅助数据（如果有时间的话重构删掉）
        public CourseData TemporaryCourse;
        public bool HasEnoughData = true;

        //教师列表
        public static List<TeacherData> totalTeacherList = new List<TeacherData>();

        //重载方法
        public string ToString(string format)
        {
            switch (format)
            {
                case ("Score"):
                    if (HasEnoughData)
                        return Score.ToString("f2");
                    else
                        return "N/A";
                case ("VoteNum"):
                    if (HasEnoughData)
                        return VoteNum.ToString();
                    else
                        return "<5";
                case ("CommentNum"):
                    if (this.HasEnoughData)
                        return CommentNum.ToString();
                    else
                        return "<5";
                case ("HotNum"):
                    if (HasEnoughData)
                    {
                        string hotNum_string = (CommentNum + VoteNum).ToString();
                        return hotNum_string;
                    }
                    else
                        return "<10";
                case ("CallNameRate"):
                    if (HasEnoughData)
                    {
                        string callNameRate_string = $"{CallNameRate * 100:N1}%";
                        return callNameRate_string;
                    }
                    else
                        return "N/A";
                default:
                    throw new Exception("Undefined Behavior");
            }
        }//重载的ToString()方法，用于打印数据
    }

    public class CommentData
    {
        public string Text;
        public DateTime Time;
        public int Vote;
        ///
        ///Next Stage
        ///
        //public enum Keyword;
    }

    public class CourseData
    {
        public string Name;
        public List<TeacherData> CourseTeachers;
        public double OverallGPAOfThisCourse;//所有老师的总GPA
        public int TotalSampleSize;//所有老师的总样本量
        public double TotalGPA;//中间量
        public double OverallGPAOfTeacher;//某个老师的GPA
        public int GPASampleSizeOfTeacher_int;//某个老师的总样本量
        public string GPASampleSizeOfTeacher_string;
        public bool HasEnoughData = true;

        public static List<CourseData> courseDataList = new List<CourseData>();
    }

    /// <summary>
    /// TODO
    /// </summary>
    public class FacultyData
    {
        public string Name;
        public int TeacherCount = 0;
        public enum FacultyType
        {
            Engineer,
            Science,
            Humanity,
            Else,
        };
        public FacultyType facultyType;//

        public List<TeacherData> TeacherList = new List<TeacherData>();

        public static List<FacultyData> FacultyList = new List<FacultyData>();

        public static FacultyData GetFaculty(TeacherData teacher, string name)
        {
            FacultyData thisFaculty;
            if ((thisFaculty = FacultyList.Where(f => f.Name == name).FirstOrDefault()) != null)
            {
                thisFaculty.TeacherList.Add(teacher);
                thisFaculty.TeacherCount++;
                return thisFaculty;
            }
            else
            {
                var newFaculty = new FacultyData { Name = name, TeacherCount = 1 };
                newFaculty.TeacherList.Add(teacher);
                FacultyList.Add(newFaculty);
                return newFaculty;
            }
        }
    }

    class GetData
    {
        public void GetTeacherDataFromCsv(FileStream file)
        {
            TeacherData.totalTeacherList.Clear(); //读取前先清空当前已读取的内容
            CourseData.courseDataList.Clear();
            FacultyData.FacultyList.Clear();
            using (StreamReader streamReader = new StreamReader(file, Encoding.Default))
            {
                string teacherLine = string.Empty;
                while ((teacherLine = streamReader.ReadLine()) != null)
                {
                    var Line = teacherLine.Split(',');
                    TeacherData thisTeacher = new TeacherData()
                    {
                        Name = Line[0],
                        ID = int.Parse(Line[1]),
                        Url = Line[2],
                    };
                    thisTeacher.Faculty = FacultyData.GetFaculty(thisTeacher, Line[3]);
                    thisTeacher.Score = (Line[4] == "N/A") ? 0 : double.Parse(Line[4]);
                    thisTeacher.CallNameRate = double.Parse(Line[5].Replace("%", "")) / 100.0;
                    thisTeacher.VoteNum = (Line[6] == "<5") ? 0 : int.Parse(Line[6]);
                    thisTeacher.CommentNum = (Line[7] == "<5") ? 0 : int.Parse(Line[7]);

                    if (thisTeacher.VoteNum < 5)
                    {
                        thisTeacher.HasEnoughData = false;
                        thisTeacher.CallNameState = TeacherData.CallName_enum.Unknown;
                    }
                    else if (thisTeacher.CallNameRate > 0.5)
                        thisTeacher.CallNameState = TeacherData.CallName_enum.Yes;
                    else if (thisTeacher.CallNameRate > 0.2)
                        thisTeacher.CallNameState = TeacherData.CallName_enum.Possible;
                    else
                        thisTeacher.CallNameState = TeacherData.CallName_enum.No;

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
                            Name = courseName,
                            GPASampleSizeOfTeacher_string = overallGPASize_string,
                            GPASampleSizeOfTeacher_int = overallGPASize_int,
                            OverallGPAOfTeacher = overallGPA_number
                        };
                        hisCourseList.Add(thisCourseData);
                        var query = from c in CourseData.courseDataList//需要debug
                                    where c.Name == courseName
                                    select c;

                        if (!query.Any())
                        {
                            var courseData = new CourseData();
                            courseData.Name = courseName;
                            courseData.CourseTeachers = new List<TeacherData>();
                            courseData.CourseTeachers.Add(thisTeacher);
                            courseData.TotalGPA += overallGPA_number * overallGPASize_int;
                            courseData.TotalSampleSize += overallGPASize_int;
                            courseData.OverallGPAOfThisCourse = courseData.TotalGPA / courseData.TotalSampleSize;
                            CourseData.courseDataList.Add(courseData);
                        }
                        else
                        {
                            foreach (var courseData in CourseData.courseDataList.Select(x => x).Where(x => x.Name == courseName))
                            {
                                courseData.CourseTeachers.Add(thisTeacher);
                                courseData.TotalGPA += overallGPA_number * overallGPASize_int;
                                courseData.TotalSampleSize += overallGPASize_int;
                                courseData.OverallGPAOfThisCourse = courseData.TotalGPA / courseData.TotalSampleSize;
                            }
                        }
                    }
                    thisTeacher.CourseList = hisCourseList;
                    var commentString = streamReader.ReadLine();
                    var comments = commentString.Split(',');
                    var count = comments.Count();
                    for (int i = 0; i < count - 1; i = i + 3)
                    {
                        thisTeacher.CommentList.Add(new CommentData()
                        {
                            Text = comments[i].Replace("&NewLine", Environment.NewLine),
                            Vote = int.Parse(comments[i + 1]),
                            Time = DateTime.Parse(comments[i + 2])
                        });
                    }
                    TeacherData.totalTeacherList.Add(thisTeacher);
                }
            }
        }

        public void SaveAllComments(string path = "Comments.txt")
        {
            var sw = new StreamWriter(path:path, append:false);
            foreach (var teacher in TeacherData.totalTeacherList)
            {
                if (teacher.CommentList.Count == 0)
                    continue;
                sw.WriteLine($"###Name:{teacher.Name}");
                foreach(var comment in teacher.CommentList)
                {
                    sw.WriteLine(comment.Text.Replace(Environment.NewLine, "&NewLine"));
                }
            }
            sw.Close();
        }
    }

    public class SearchAlgorithm
    {
        public List<TeacherData> SearchTeacherName(string keyword, int maxResults, bool hideUnrated, Config.ShowOrder showOrder = Config.ShowOrder.None)
        {
            IEnumerable<TeacherData> teacherList;
            if (!hideUnrated)
            {
                teacherList =  TeacherData.totalTeacherList.Where(t => t.Name.ToUpper().Contains(keyword.ToUpper()));
            }
            else
            {
                teacherList = TeacherData.totalTeacherList.Where(t => t.HasEnoughData).Where(t => t.Name.ToUpper().Contains(keyword.ToUpper()));
            }
            switch (Config.Order)
            {
                case (Config.ShowOrder.Score):
                    teacherList = teacherList.OrderByDescending(t => t.Score);
                    break;
                case (Config.ShowOrder.HotNum):
                    teacherList = teacherList.OrderByDescending(t => t.CommentNum + t.VoteNum);
                    break;
                case (Config.ShowOrder.CallNameRate):
                    teacherList = teacherList.OrderBy(t => t.CallNameState);
                    break;
                case (Config.ShowOrder.None):
                    break;
            }
            return teacherList.ToList();
        }

        public List<CourseData> SearchCourseName(string keyword, int maxResults, bool hideUnrated, Config.ShowOrder showOrder = Config.ShowOrder.None)
        {
            IEnumerable<CourseData> courseList;
            if (!hideUnrated)
            {
                courseList = CourseData.courseDataList.Where(c => c.Name.ToUpper().Contains(keyword.ToUpper()));
            }
            else
            {
                courseList = CourseData.courseDataList.Where(c => c.HasEnoughData).Where(c => c.Name.ToUpper().Contains(keyword.ToUpper()));
            }
            switch (Config.Order)
            {
                case (Config.ShowOrder.Score):
                    courseList = courseList.OrderByDescending(c => c.OverallGPAOfThisCourse);
                    break;
                case (Config.ShowOrder.HotNum):
                    courseList = courseList.OrderByDescending(c => c.TotalSampleSize);
                    break;
                case (Config.ShowOrder.None):
                    break;
                default:
                    break;
            }
            return courseList.ToList();
        }
    }
}