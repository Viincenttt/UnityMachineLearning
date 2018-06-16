using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts {
    public class DNA {
        private List<CharacterAction> Genes { get; set; } = new List<CharacterAction>();
        private int _dnaLength;
        private int _maxValues = 5;

        public DNA(int length, int maxValues) {
            this._dnaLength = length;
            this._maxValues = maxValues;
            this.SetRandom();
        }

        public void SetRandom() {
            this.Genes.Clear();
            for (int i = 0; i < this._dnaLength; i++) {
                this.Genes.Add((CharacterAction)Random.Range(0, this._maxValues));
            }
        }

        public CharacterAction GetGene(int position) {
            return this.Genes[position];
        }

        public void SetGene(int position, CharacterAction value) {
            this.Genes[position] = value;
        }

        public void Combine(DNA dna1, DNA dna2) {
            for (int i = 0; i < this._dnaLength; i++) {
                if (i < this._dnaLength / 2f) {
                    this.Genes[i] = dna1.Genes[i];
                }
                else {
                    this.Genes[i] = dna2.Genes[i];
                }
            }
        }

        public void Mutate() {
            this.Genes[Random.Range(0, this._dnaLength)] = (CharacterAction)Random.Range(0, this._maxValues);
        }
    }
}
