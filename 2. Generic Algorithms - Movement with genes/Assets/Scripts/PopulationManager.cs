﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts {
    public class PopulationManager : MonoBehaviour {
        [SerializeField] private GameObject _characterToSpawn;
        [SerializeField] private int _populationSize = 1;

        [Header("Spawn position bounds")]
        [SerializeField] private float _xOffSetSpawningPosition = 2;
        [SerializeField] private float _yOffSetSpawningPosition = -2;

        private float _elapsedTime = 0;
        private float _trialTime = 5;
        private int _currentGeneration = 1;

        private GUIStyle _guiStyle = new GUIStyle();
        
        void Start () {
            //this._elapsedTime.Value = 0;
            this.SpawnInitialPopulation();
        }
	
        void Update () {
            this._elapsedTime += Time.deltaTime;
            if (this._elapsedTime > this._trialTime) {
                this.BreedNewPopulation();
                this._elapsedTime = 0;
            }
        }

        private void SpawnInitialPopulation() {
            for (int i = 0; i < this._populationSize; i++) {
                Vector3 startingPosition = this.GetNewSpawningPosition();
                GameObject b = GameObject.Instantiate(this._characterToSpawn, startingPosition, this.transform.rotation, this.transform);
                b.GetComponent<Brain>().Init();
            }
        }

        private GameObject BreedNewCharacter(Brain parent1, Brain parent2) {
            Vector3 startingPosition = this.GetNewSpawningPosition();
            GameObject offspring = GameObject.Instantiate(this._characterToSpawn, startingPosition, this.transform.rotation, this.transform);
            Brain offspringBrain = offspring.GetComponent<Brain>();

            // TODO: Apply mutation sometimes

            offspringBrain.Init();
            offspringBrain.DNA.Combine(parent1.DNA, parent2.DNA);
            return offspring;
        }

        private void BreedNewPopulation() {
            // Order the population by the time before they died
            List<Brain> population = this.GetCurrentPopulation().OrderBy(x => x.TimeBeforeDeath).ToList();

            // Breed the fittest half of the population
            for (int i = (int)(population.Count / 2.0f) - 1; i < population.Count - 1; i++) {
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

        private Vector3 GetNewSpawningPosition() {
            return new Vector3(
                this.transform.position.x + Random.Range(-this._xOffSetSpawningPosition, this._xOffSetSpawningPosition),
                this.transform.position.y,
                this.transform.position.z + Random.Range(-this._yOffSetSpawningPosition, this._xOffSetSpawningPosition));
        }

        private void OnGUI() {
            this._guiStyle.fontSize = 25;
            this._guiStyle.normal.textColor = Color.white;

            GUI.Label(new Rect(10, 10, 100, 20), $"Generation: {this._currentGeneration}", this._guiStyle);
            GUI.Label(new Rect(10, 65, 100, 20), $"Trial time: {(int)this._elapsedTime}", this._guiStyle);
        }
    }
}