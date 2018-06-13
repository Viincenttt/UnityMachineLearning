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
        [SerializeField] private FloatVariable _elapsedTime;

        [Header("Spawn position bounds")]
        [SerializeField] private float _minimumXSpawnPosition = -8;
        [SerializeField] private float _maximumXSpawnPosition = 8;
        [SerializeField] private float _minimumYSpawnPosition = -4.5f;
        [SerializeField] private float _maximumYSpawnPosition = 4.5f;

        private int _currentGeneration = 1;

        List<GameObject> population=  new List<GameObject>();
        private GUIStyle guiStyle = new GUIStyle();

        private void Start() {
            this.SpawnInitialPopulation();
        }

        private void Update() {
            this._elapsedTime.Value += Time.deltaTime;
            if (this._elapsedTime.Value > this._trialTime) {
                this.BreedNewPopulation();
                this._elapsedTime.Value = 0;
            }
        }

        private void SpawnInitialPopulation() {
            for (int i = 0; i < this._populationSize; i++) {
                this.SpawnPerson(Random.Range(0.0f, 1f), Random.Range(0.0f, 1f), Random.Range(0.0f, 1f));
            }
        }

        private void BreedNewPopulation() {
            List<GameObject> sortedPopulation = this.population.OrderBy(x => x.GetComponent<DNA>().TimeBeforeDeath).ToList();

            this.population.Clear();

            for (int i = (int)(sortedPopulation.Count / 2.0f) - 1; i < sortedPopulation.Count - 1; i++) {
                this.BreedPerson(sortedPopulation[i], sortedPopulation[i + 1]);
                this.BreedPerson(sortedPopulation[i + 1], sortedPopulation[i]);
            }
            
            for (int i = 0; i < sortedPopulation.Count; i++) {
                GameObject.Destroy(sortedPopulation[i]);
            }

            this._currentGeneration++;
        }

        private void BreedPerson(GameObject parentA, GameObject parentB) {
            DNA dnaParentA = parentA.GetComponent<DNA>();
            DNA dnaParentB = parentB.GetComponent<DNA>();

            float r = (Random.Range(0, 2) == 0) ? dnaParentA.R : dnaParentB.R;
            float g = (Random.Range(0, 2) == 0) ? dnaParentA.G : dnaParentB.G;
            float b = (Random.Range(0, 2) == 0) ? dnaParentA.B : dnaParentB.B;

            this.SpawnPerson(r, g, b);
            /*
            bool shouldMutate = (Random.Range(0, 2) == 0);
            if (shouldMutate) {
                offspringDNA.r = Random.Range(0.0f, 1f);
                offspringDNA.g = Random.Range(0.0f, 1f);
                offspringDNA.b = Random.Range(0.0f, 1f);
            }
            else {
                
            }*/
        }

        private void SpawnPerson(float r, float g, float b) {
            Vector3 position = this.GetRandomSpawnPosition();
            GameObject personToSpawn = GameObject.Instantiate(this._personToSpawn, position, Quaternion.identity);
            DNA dnaObject = personToSpawn.GetComponent<DNA>();
            dnaObject.R = r;
            dnaObject.G = g;
            dnaObject.B = b;

            this.population.Add(personToSpawn);
        }

        private Vector3 GetRandomSpawnPosition() {
            return new Vector3(Random.Range(this._minimumXSpawnPosition, this._maximumXSpawnPosition), Random.Range(this._minimumYSpawnPosition, this._maximumYSpawnPosition), 0);
        }

        private void OnGUI() {
            this.guiStyle.fontSize = 50;
            this.guiStyle.normal.textColor = Color.white;
            GUI.Label(new Rect(10, 10, 100, 20), $"Generation: {this._currentGeneration}", this.guiStyle);
            GUI.Label(new Rect(10, 65, 100, 20), $"Trial time: {(int)this._elapsedTime.Value}", this.guiStyle);
        }
    }
}