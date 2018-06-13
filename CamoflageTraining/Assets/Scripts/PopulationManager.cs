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
        [SerializeField] private float _minimumSize = 0.1f;
        [SerializeField] private float _maximumSize = 0.25f;

        [Header("Spawn position bounds")]
        [SerializeField] private float _minimumXSpawnPosition = -8;
        [SerializeField] private float _maximumXSpawnPosition = 8;
        [SerializeField] private float _minimumYSpawnPosition = -4.5f;
        [SerializeField] private float _maximumYSpawnPosition = 4.5f;

        private int _currentGeneration = 1;

        private GUIStyle guiStyle = new GUIStyle();

        private void Start() {
            this._elapsedTime.Value = 0;
            this.SpawnInitialPopulation();
        }

        private void Update() {
            this._elapsedTime.Value += Time.deltaTime;
            if (this._elapsedTime.Value > this._trialTime) {
                this.BreedNewPopulation();
            }
        }

        private void SpawnInitialPopulation() {
            for (int i = 0; i < this._populationSize; i++) {
                this.SpawnPerson(Random.Range(0.0f, 1f), Random.Range(0.0f, 1f), Random.Range(0.0f, 1f), Random.Range(this._minimumSize, this._maximumSize));
            }
        }

        private void BreedNewPopulation() {
            // Order the population by the time before they died
            List<DNA> population = this.GetCurrentPopulation().OrderBy(x => x.TimeBeforeDeath).ToList();

            // Breed the fittest half of the population
            for (int i = (int)(population.Count / 2.0f) - 1; i < population.Count - 1; i++) {
                this.BreedPerson(population[i], population[i + 1]);
                this.BreedPerson(population[i + 1], population[i]);
            }

            // Destroy the previous population
            foreach (DNA person in population) {
                GameObject.Destroy(person.gameObject);
            }

            this._elapsedTime.Value = 0;
            this._currentGeneration++;
        }

        private void BreedPerson(DNA dnaParentA, DNA dnaParentB) {
            float r = (Random.Range(0, 2) == 0) ? dnaParentA.R : dnaParentB.R;
            float g = (Random.Range(0, 2) == 0) ? dnaParentA.G : dnaParentB.G;
            float b = (Random.Range(0, 2) == 0) ? dnaParentA.B : dnaParentB.B;
            float scale = (Random.Range(0, 2) == 0) ? dnaParentA.Scale : dnaParentB.Scale;

            this.SpawnPerson(r, g, b, scale);
        }

        private void SpawnPerson(float r, float g, float b, float scale) {
            Vector3 position = this.GetRandomSpawnPosition();
            GameObject personToSpawn = GameObject.Instantiate(this._personToSpawn, position, Quaternion.identity, this.transform);
            DNA dnaObject = personToSpawn.GetComponent<DNA>();
            dnaObject.R = r;
            dnaObject.G = g;
            dnaObject.B = b;
            dnaObject.Scale = scale;
        }

        private DNA[] GetCurrentPopulation() {
            return this.GetComponentsInChildren<DNA>();
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