﻿


namespace GrolExamGenerator
{
    public class clsQuestionInfo
    {
        public string QuestionText = "";
        public string AnswerA = "";
        public string AnswerB = "";
        public string AnswerC = "";
        public string AnswerD = "";
        public string[] CorrectAnswerLetter = { "", "" }; // [correct ans, student ans]
        public string QuestionNumber = "";
        public bool IsLearnMode = false;
    }  // end class
} // end namespace
