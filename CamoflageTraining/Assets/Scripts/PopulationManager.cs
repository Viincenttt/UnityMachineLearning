using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts {
    public class PopulationManager : MonoBehaviour {
        [Header("Spawning configuration")]
        [SerializeField] private GameObject _personToSpawn;
        [SerializeField] private int _populationSize = 10;
        [SerializeField] private int _trialTime = 10;

        [Header("Spawn position bounds")]
        [SerializeField] private float _minimumXSpawnPosition = -8;
        [SerializeField] private float _maximumXSpawnPosition = 8;
        [SerializeField] private float _minimumYSpawnPosition = -4.5f;
        [SerializeField] private float _maximumYSpawnPosition = 4.5f;

        private int _currentGeneration = 1;

        List<GameObject> population=  new List<GameObject>();
        public static float elapsedTime = 0;
        private GUIStyle guiStyle = new GUIStyle();

        private void Start() {
            for (int i = 0; i < this._populationSize; i++) {
                Vector3 position = this.GetRandomSpawnPosition();
                GameObject personToSpawn = GameObject.Instantiate(this._personToSpawn, position, Quaternion.identity);
                DNA dnaObject = personToSpawn.GetComponent<DNA>();
                dnaObject.R = Random.Range(0.0f, 1f);
                dnaObject.G = Random.Range(0.0f, 1f);
                dnaObject.B = Random.Range(0.0f, 1f);
                this.population.Add(personToSpawn);
            }
        }

        private void Update() {
            elapsedTime += Time.deltaTime;
            if (elapsedTime > this._trialTime) {
                this.BreedNewPopulation();
                elapsedTime = 0;
            }
        }

        private void BreedNewPopulation() {
            List<GameObject> sortedPopulation = this.population.OrderBy(x => x.GetComponent<DNA>().TimeBeforeDeath).ToList();

            this.population.Clear();


            for (int i = (int)(sortedPopulation.Count / 2.0f) - 1; i < sortedPopulation.Count - 1; i++) {
                this.population.Add(this.Breed(sortedPopulation[i], sortedPopulation[i + 1]));
                this.population.Add(this.Breed(sortedPopulation[i + 1], sortedPopulation[i]));
            }

            

            for (int i = 0; i < sortedPopulation.Count; i++) {
                GameObject.Destroy(sortedPopulation[i]);
            }

            this._currentGeneration++;
        }

        private GameObject Breed(GameObject parentA, GameObject parentB) {
            Vector3 spawnPosition = this.GetRandomSpawnPosition();
            GameObject offspring = GameObject.Instantiate(this._personToSpawn, spawnPosition, Quaternion.identity);
            DNA dnaParentA = parentA.GetComponent<DNA>();
            DNA dnaParentB = parentB.GetComponent<DNA>();
            DNA offspringDNA = offspring.GetComponent<DNA>();
            offspringDNA.R = (Random.Range(0, 2) == 0) ? dnaParentA.R : dnaParentB.R;
            offspringDNA.G = (Random.Range(0, 2) == 0) ? dnaParentA.G : dnaParentB.G;
            offspringDNA.B = (Random.Range(0, 2) == 0) ? dnaParentA.B : dnaParentB.B;
            /*
            bool shouldMutate = (Random.Range(0, 2) == 0);
            if (shouldMutate) {
                offspringDNA.r = Random.Range(0.0f, 1f);
                offspringDNA.g = Random.Range(0.0f, 1f);
                offspringDNA.b = Random.Range(0.0f, 1f);
            }
            else {
                
            }*/

            return offspring;
        }

        private Vector3 GetRandomSpawnPosition() {
            return new Vector3(Random.Range(this._minimumXSpawnPosition, this._maximumXSpawnPosition), Random.Range(this._minimumYSpawnPosition, this._maximumYSpawnPosition), 0);
        }

        private void OnGUI() {
            this.guiStyle.fontSize = 50;
            this.guiStyle.normal.textColor = Color.white;
            GUI.Label(new Rect(10, 10, 100, 20), "Generation: " + this._currentGeneration, this.guiStyle);
            GUI.Label(new Rect(10, 65, 100, 20), "Trial time: " + (int)elapsedTime, this.guiStyle);
        }
    }
}