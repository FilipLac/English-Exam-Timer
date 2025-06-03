namespace English_Exam_Timer
{
    public class PhaseTime
    {
        public string Name { get; set; } = string.Empty;
        public int DurationSeconds { get; set; }

        public PhaseTime() { }

        public PhaseTime(string name, int duration)
        {
            Name = name;
            DurationSeconds = duration;
        }
    }
}
