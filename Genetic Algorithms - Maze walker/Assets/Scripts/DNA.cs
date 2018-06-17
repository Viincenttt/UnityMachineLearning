using UnityEngine;

namespace Assets.Scripts {
    public class DNA {
        public float TurningAngle { get; set; }

        public DNA() {
            this.TurningAngle = Random.Range(0, 360);
        }

        public static DNA Combine(DNA dna1, DNA dna2) {
            DNA offspringDna = new DNA();
            offspringDna.TurningAngle = Random.Range(0, 2) == 0 ? dna1.TurningAngle : dna2.TurningAngle;

            return offspringDna;
        }
    }
}