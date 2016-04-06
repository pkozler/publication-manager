using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public sealed class QualificationThesisType
    {
        private readonly string name;
        private readonly int value;

        public static readonly QualificationThesisType MASTER_THESIS = new QualificationThesisType("Diplomová práce", 1);
        public static readonly QualificationThesisType PHD_THESIS = new QualificationThesisType("Disertační práce", 2);

        private QualificationThesisType(string name, int value)
        {
            this.name = name;
            this.value = value;
        }

        public override string ToString()
        {
            return name;
        }
    }
}
