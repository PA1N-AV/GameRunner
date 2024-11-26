namespace Runer
{
    class Player
    {
        public int X { get; set; }
        public int Y { get; set; }
        private MapSize mapSize;

        public Player(MapSize mapSize)
        {
            this.mapSize = mapSize;
        }

        public void Move(Offset offset)
        {
            if ((X + offset.X < 0) || (X + offset.X >= mapSize.Width) || (Y + offset.Y < 0) || (Y + offset.Y >= mapSize.Height))
            {
                return;
            }

            X += offset.X;
            Y += offset.Y;
        }

    }
}