namespace CypherSheet.Domain
{
    public class RollCalculation
    {
        public int MightEffort { get; set; } = 0;
        public int SpeedEffort { get; set; } = 0;
        public int IntellectEffort { get; set; } = 0;
        public int Bonus { get; set; } = 0;
        public int Penalty { get; set; } = 0;
        public int AbilityCost { get; set; } = 0;

        public int TotalEffort => MightEffort + SpeedEffort + IntellectEffort;
        
        public int CalculateEffortCost()
        {
            if (TotalEffort == 0) return 0;
            return (TotalEffort * 2) + 1;
        }
        
        public int CalculateMightCost(int mightEdge, bool impaired)
        {
            if (MightEffort == 0) return 0;
            int baseCost = (MightEffort * 2) + 1;
            int impairedPenalty = impaired ? MightEffort : 0;
            int finalCost = baseCost + impairedPenalty - mightEdge;
            return Math.Max(0, finalCost);
        }
        
        public int CalculateSpeedCost(int speedEdge, bool impaired)
        {
            if (SpeedEffort == 0) return 0;
            int baseCost = (SpeedEffort * 2) + 1;
            int impairedPenalty = impaired ? SpeedEffort : 0;
            int finalCost = baseCost + impairedPenalty - speedEdge;
            return Math.Max(0, finalCost);
        }
        
        public int CalculateIntellectCost(int intellectEdge, bool impaired)
        {
            if (IntellectEffort == 0) return 0;
            int baseCost = (IntellectEffort * 2) + 1;
            int impairedPenalty = impaired ? IntellectEffort : 0;
            int finalCost = baseCost + impairedPenalty - intellectEdge;
            return Math.Max(0, finalCost);
        }
        
        public int CalculateTotalCost(Character character)
        {
            int mightCost = CalculateMightCost(character.Might.Edge, character.Impaired);
            int speedCost = CalculateSpeedCost(character.Speed.Edge, character.Impaired);
            int intellectCost = CalculateIntellectCost(character.Intellect.Edge, character.Impaired);
            
            return mightCost + speedCost + intellectCost + AbilityCost;
        }
        
        public bool CanAfford(Character character)
        {
            int mightCost = CalculateMightCost(character.Might.Edge, character.Impaired) + AbilityCost;
            int speedCost = CalculateSpeedCost(character.Speed.Edge, character.Impaired);
            int intellectCost = CalculateIntellectCost(character.Intellect.Edge, character.Impaired);
            
            return character.Might.Current >= mightCost && 
                   character.Speed.Current >= speedCost && 
                   character.Intellect.Current >= intellectCost;
        }
    }
}