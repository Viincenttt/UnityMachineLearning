using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts;
using Random = UnityEngine.Random;

public class DNA {
    public IDictionary<KnownSituation, KnownAction> SituationMap = new Dictionary<KnownSituation, KnownAction>();
    
    public static DNA CreateRandomDna() {
        DNA randomDna = new DNA();
        foreach (KnownSituation situation in AllKnownSituations) {
            KnownAction randomAction = AllKnownActions[Random.Range(0, AllKnownActions.Count())];
            randomDna.SituationMap[situation] = randomAction;
        }

        return randomDna;
    }
    
	public static DNA Combine(DNA parentDna1, DNA parentDna2) {
        DNA offspringDna = new DNA();
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
