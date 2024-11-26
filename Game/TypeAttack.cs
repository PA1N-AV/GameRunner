namespace Runer
{
    class TypeAttack
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Attack Attack { get; set; }
        public string Type { get; set; }
        public int Time { get; set; }
        public int AllTime { get; set; }

        public TypeAttack(int x, int y, Attack attack, string type, int time, int allTime)
        {
            X = x;
            Y = y;
            Attack = attack;
            Type = type;
            Time = time;
            AllTime = allTime;
        }
    }
}