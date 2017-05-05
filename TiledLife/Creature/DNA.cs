using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TiledLife.Creature
{
    // Represents every unique attribute for a humna
    class DNA
    {
        public enum PhysicalAttribute {
            ThirstIncreaseRate, MaxThirst, ThirstThreshold,
            ViewDistance,
            RotateSpeed, WalkSpeed
        };

        Dictionary<PhysicalAttribute, float> physicalAttributes;

        public DNA()
        {
            physicalAttributes = new Dictionary<PhysicalAttribute, float>();

            // Thirst attributes
            physicalAttributes.Add(PhysicalAttribute.ThirstIncreaseRate, RandomGen.GetFloat(0.95f, 1f)); 
            physicalAttributes.Add(PhysicalAttribute.MaxThirst, RandomGen.GetInstance().Next(98, 103)); 
            physicalAttributes.Add(PhysicalAttribute.ThirstThreshold, RandomGen.GetFloat(0.01f, 0.03f)); //TODO set to something higher

            physicalAttributes.Add(PhysicalAttribute.ViewDistance, RandomGen.GetInstance().Next(50, 75));

            // Speed attributes
            physicalAttributes.Add(PhysicalAttribute.RotateSpeed, RandomGen.GetFloat(0.50f, 3.0f));
            physicalAttributes.Add(PhysicalAttribute.WalkSpeed, RandomGen.GetFloat(0.08f, 0.12f));
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
