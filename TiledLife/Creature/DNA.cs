using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TiledLife.Creature
{
    // Represents every unique attribute for a humna
    class DNA
    {
        public enum PhysicalAttributes { thirstIncreaseRate, maxThirst };

        Dictionary<PhysicalAttributes, float> physicalAttributes;

        public DNA()
        {
            physicalAttributes = new Dictionary<PhysicalAttributes, float>();

            physicalAttributes.Add(PhysicalAttributes.thirstIncreaseRate, ((float)RandomSingleton.GetRandom().Next(95, 100)/50));
            physicalAttributes.Add(PhysicalAttributes.maxThirst, RandomSingleton.GetRandom().Next(98, 103));
        }

        // Create DNA from two parents
        public DNA(DNA dna1, DNA dna2)
        {
            foreach(PhysicalAttributes attribute in Enum.GetValues(typeof(PhysicalAttributes)))
            {
                // Select value from either parent
                float value1 = dna1.GetPhysicalAttr(attribute);
                float value2 = dna2.GetPhysicalAttr(attribute);
                float chosenValue = RandomSingleton.GetRandom().Next(0, 2) == 0 ? value1 : value2;

                // Add random variance, maximum of 5%.
                float finalValue = chosenValue * (RandomSingleton.GetRandom().Next(95, 106) / 100);

                physicalAttributes.Add(attribute, finalValue);
            }
        }

        public float GetPhysicalAttr(PhysicalAttributes name)
        {
            return physicalAttributes[name];
        }
    }
}
