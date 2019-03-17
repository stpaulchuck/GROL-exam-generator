

namespace GrolExamGenerator
{

    /***************************************** selection chhoice event *****************************************************/

    public class clsGROLChoiceEventArgs
    {
        public string QuestionNumber;
        public string ChoiceLetter;

        public clsGROLChoiceEventArgs() { }

        public clsGROLChoiceEventArgs(string Qnum, string Choice)
        {
            QuestionNumber = Qnum;
            ChoiceLetter = Choice;
        }
    }

    public delegate void ExamFormChoiceEvent(clsGROLChoiceEventArgs args);

} // end namespace
