using System;
using System.Linq;
using Random = UnityEngine.Random;

namespace Assets.Scripts {
    public class DNA {
        public CharacterAction PrimaryAction { get; set; }
        public CharacterAction SecondaryAction { get; set; }

        public DNA() {
            this.PrimaryAction = (CharacterAction)Random.Range(0, (int)Enum.GetValues(typeof(CharacterAction)).Cast<CharacterAction>().Max() + 1);
            this.SecondaryAction = (CharacterAction)Random.Range(0, (int)Enum.GetValues(typeof(CharacterAction)).Cast<CharacterAction>().Max() + 1);
        }

        public static DNA Combine(DNA dna1, DNA dna2) {
            DNA offspringDna = new DNA();
            offspringDna.PrimaryAction = Random.Range(0, 2) == 0 ? dna1.PrimaryAction : dna2.PrimaryAction;
            offspringDna.SecondaryAction = Random.Range(0, 2) == 0 ? dna1.SecondaryAction : dna2.SecondaryAction;

            return offspringDna;
        }
    }
}