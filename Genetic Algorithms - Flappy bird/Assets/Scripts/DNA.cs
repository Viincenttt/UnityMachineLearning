using System;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

namespace Assets.Scripts {
    public class DNA {
        public IDictionary<KnownSituation, KnownAction> SituationMap = new Dictionary<KnownSituation, KnownAction>();
        public float MaximumSpeed;
        public float ViewingDistance;

        public static DNA CreateRandomDna() {
            DNA randomDna = new DNA();
            randomDna.MaximumSpeed = Random.Range(1, 6);
            randomDna.ViewingDistance = Random.Range(1, 8);
            foreach (KnownSituation situation in AllKnownSituations) {
                KnownAction randomAction = AllKnownActions[Random.Range(0, AllKnownActions.Count())];
                randomDna.SituationMap[situation] = randomAction;
            }

            return randomDna;
        }
    
        public static DNA Combine(DNA parentDna1, DNA parentDna2) {
            DNA offspringDna = new DNA();
            offspringDna.MaximumSpeed = Random.Range(0, 2) == 0 ? parentDna1.MaximumSpeed : parentDna2.MaximumSpeed;
            offspringDna.ViewingDistance = Random.Range(0, 2) == 0 ? parentDna1.ViewingDistance : parentDna2.ViewingDistance;
            foreach (KnownSituation situation in AllKnownSituations) {
                offspringDna.SituationMap[situation] = Random.Range(0, 2) == 0 ? parentDna1.SituationMap[situation] : parentDna2.SituationMap[situation];
            }

            return offspringDna;
        }
    
        public void Mutate() {
            KnownSituation randomSituation = AllKnownSituations[Random.Range(0, AllKnownSituations.Count())];
            KnownAction randomAction = AllKnownActions[Random.Range(0, AllKnownActions.Count())];

            this.SituationMap[randomSituation] = randomAction;
        }

        private static KnownSituation[] AllKnownSituations => Enum.GetValues(typeof(KnownSituation)).Cast<KnownSituation>().ToArray();
        private static KnownAction[] AllKnownActions => Enum.GetValues(typeof(KnownAction)).Cast<KnownAction>().ToArray();
    }
}
