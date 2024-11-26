namespace Runer
{
    class Attack
    {

        public Attack(int danger, int time_attack)
        {
            this.Danger = danger;
            this.TimeAttack = time_attack;
        }
        public Attack(Attack attack)
        {
            this.Danger = attack.Danger;
            this.TimeAttack = attack.TimeAttack;
        }
        public int Danger { get; set; }
        public int TimeAttack { get; set; }

        public bool Tact()
        {
            if (Danger > 0)
            {
                Danger--;
                return false;
            }
            else if (TimeAttack > 0)
            {
                TimeAttack--;
                if (TimeAttack == 0)
                {
                    return true;
                }
                return false;

            }
            else { return true; }

        }
    }
}