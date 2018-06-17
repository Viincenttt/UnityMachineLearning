using System;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts {
    public class Brain : MonoBehaviour {
        [SerializeField] private readonly float _movementSpeed = 1;
        [SerializeField] private GameObject _eyes;

        private bool _isAlive;
        private bool _canSeeGround;

        private Vector3 _movementVector;
        public float TimeBeforeDeath { get; set; }
        public int WalkingActionCount { get; set; }
        public DNA DNA { get; set; }

        public void Init() {
            this.TimeBeforeDeath = 0;
            this._isAlive = true;
            this.DNA = new DNA();
        }

        private void Update() {
            if (!this._isAlive) {
                return;
            }

            this.TimeBeforeDeath += Time.deltaTime;

            if (this.CanSeeGround()) {
                this.PerformAction(this.DNA.PrimaryAction);
            }
            else {
                this.PerformAction(this.DNA.SecondaryAction);
            }
        }

        private void PerformAction(CharacterAction action) {
            float move = 0;
            float turn = 0;
            switch (action) {
                case CharacterAction.Forward:
                    move = 1;
                    this.WalkingActionCount += 1;
                    break;
                case CharacterAction.Left:
                    turn = -90;
                    break;
                case CharacterAction.Right:
                    turn = 90;
                    break;
            }

            this.transform.Translate(0, 0, move * 0.1f);
            this.transform.Rotate(0, turn, 0);
        }

        private bool CanSeeGround() {
            RaycastHit raycastHit;
            if (Physics.Raycast(this._eyes.transform.position, this._eyes.transform.forward * 10, out raycastHit)) {
                if (raycastHit.collider.gameObject.tag == KnownTags.Platform) {
                    return true;
                }
            }

            return false;
        }

        private void OnCollisionEnter(Collision obj) {
            if (obj.gameObject.tag == KnownTags.Dead) {
                this._isAlive = false;
            }
        }
    }
}