using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts {
    public class PopulationManager : MonoBehaviour {
        [SerializeField] private GameObject _characterToSpawn;
        [SerializeField] private GameObject _spawnPosition;
        [SerializeField] private float _populationSize = 100;
        [SerializeField] private float _trialTime = 10;

        private float _timeElapsed = 0;
        private int _currentGeneration = 1;
        private GUIStyle _guiStyle = new GUIStyle();

        void Start () {
            this.SpawnInitialPopulation();
        }

        private void SpawnInitialPopulation() {
            for (int i = 0; i < this._populationSize; i++) {
                Brain brain = this.BreedNewCharacter();
                brain.DNA = DNA.CreateRandomDna();
                brain.Start();
            }
        }

        private void BreedNewCharacter(Brain parent1, Brain parent2) {
            Brain offspringBrain = this.BreedNewCharacter();
            offspringBrain.DNA = DNA.Combine(parent1.DNA, parent2.DNA);
        }

        private Brain BreedNewCharacter() {
            GameObject newCharacter = GameObject.Instantiate(this._characterToSpawn, this._spawnPosition.transform.position, this.transform.rotation, this.transform);
            Brain brain = newCharacter.GetComponent<Brain>();
            brain.Start();
            return brain;
        }

        void BreedNewPopulation() {
            // Order the population by the time before they died
            List<Brain> population = this.GetCurrentPopulation().OrderByDescending(x => x.DistanceTravelled - x.NumberOfCrashes).ToList();

            // Breed the fittest half of the population
            for (int i = 0; i < (population.Count / 2); i++) {
                this.BreedNewCharacter(population[i], population[i + 1]);
                this.BreedNewCharacter(population[i + 1], population[i]);
            }

            // Destroy the previous population
            foreach (Brain dna in population) {
                GameObject.Destroy(dna.gameObject);
            }

            this._currentGeneration++;
        }
    
        private Brain[] GetCurrentPopulation() {
            return this.GetComponentsInChildren<Brain>();
        }

        void Update () {
            this._timeElapsed += Time.deltaTime;
            if(this._timeElapsed >= this._trialTime) {
                this.BreedNewPopulation();
                this._timeElapsed = 0;
            }
        }
    
        private void OnGUI() {
            this._guiStyle.fontSize = 25;
            this._guiStyle.normal.textColor = Color.white;
            GUI.BeginGroup(new Rect(10, 10, 250, 150));
            GUI.Box(new Rect(0, 0, 140, 140), "Stats", this._guiStyle);
            GUI.Label(new Rect(10, 25, 200, 30), $"Generation: {this._currentGeneration}", this._guiStyle);
            GUI.Label(new Rect(10, 50, 200, 30), $"Time: {this._timeElapsed:0.00}", this._guiStyle);
            GUI.EndGroup();
        }
    }
}
