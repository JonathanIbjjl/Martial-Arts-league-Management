using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contenders
{
    class Contender
    {
        public string Name { get; set; }
        public string Fname { get; set; }
        public string ID { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string DateOfBirth { get; set; } // its not DateTime Type because format is not validated inside google docs
        public int Age { get; set; }
        public int AgeCategory { get; set; }
        public bool IsMale { get; set; }
        public int Weight { get; set; }
        public int WeightCategory { get; set; }
        public int Belt { get; set; }
        public string AcademyName { get; set; }
        public string CoachName { get; set; }
        public string CoachPhone { get; set; }
        public bool IsAllowedWeightGradeAbove { get; set; }
        public bool IsAllowedAgeGradeAbove { get; set; }
        public bool IsAllowedBeltGradeAbove { get; set; }
        public bool IsChild { get; set; }
    }
}
