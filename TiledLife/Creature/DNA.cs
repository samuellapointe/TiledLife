using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TiledLife.Creature
{
    // Represents every unique attribute for a humna
    class DNA
    {
        public enum PhysicalAttribute { ThirstIncreaseRate, MaxThirst };

        Dictionary<PhysicalAttribute, float> physicalAttributes;

        public DNA()
        {
            physicalAttributes = new Dictionary<PhysicalAttribute, float>();

            physicalAttributes.Add(PhysicalAttribute.ThirstIncreaseRate, ((float)RandomGen.GetInstance().Next(95, 100)/50));
            physicalAttributes.Add(PhysicalAttribute.MaxThirst, RandomGen.GetInstance().Next(98, 103));
        }

        // Create DNA from two parents
        public DNA(DNA dna1, DNA dna2)
        {
            foreach(PhysicalAttribute attribute in Enum.GetValues(typeof(PhysicalAttribute)))
            {
                // Select value from either parent
                float value1 = dna1.GetPhysicalAttr(attribute);
                float value2 = dna2.GetPhysicalAttr(attribute);
                float chosenValue = RandomGen.GetInstance().Next(0, 2) == 0 ? value1 : value2;

                // Add random variance, maximum of 5%.
                float finalValue = chosenValue * (RandomGen.GetInstance().Next(95, 106) / 100);

                physicalAttributes.Add(attribute, finalValue);
            }
        }

        public float GetPhysicalAttr(PhysicalAttribute name)
        {
            return physicalAttributes[name];
        }
    }
}
