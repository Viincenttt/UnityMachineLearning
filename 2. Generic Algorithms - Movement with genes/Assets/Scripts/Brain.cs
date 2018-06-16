using System;
using System.Linq;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
using Random = UnityEngine.Random;

namespace Assets.Scripts {
    [RequireComponent(typeof(ThirdPersonCharacter))]
    public class Brain : MonoBehaviour {
        [SerializeField] private float _movementSpeed = 1;
        public float TimeBeforeDeath { get; set; }
        public CharacterAction Action { get; set; }

        private ThirdPersonCharacter _thirdPersonCharacter;
        private Vector3 _movementVector;
        private bool _isAlive;

        public void Init() {
            this._thirdPersonCharacter = this.GetComponent<ThirdPersonCharacter>();
            this.TimeBeforeDeath = 0;
            this._isAlive = true;

            this.Action = (CharacterAction)Random.Range(0, (int)Enum.GetValues(typeof(CharacterAction)).Cast<CharacterAction>().Max());
        }
	
        private void FixedUpdate () {
            float horizontal = 0;
            float vertical = 0;
            switch (this.Action) {
                case CharacterAction.Forward: vertical = this._movementSpeed; break;
                case CharacterAction.Back: vertical = -this._movementSpeed; break;
                case CharacterAction.Left: horizontal = -this._movementSpeed; break;
                case CharacterAction.Right: horizontal = this._movementSpeed; break;
            }

            this._movementVector = vertical * Vector3.forward + horizontal * Vector3.right;
            this._thirdPersonCharacter.Move(this._movementVector, false, false);
            if (this._isAlive) {
                this.TimeBeforeDeath += Time.deltaTime;
            }
        }

        private void OnCollisionEnter(Collision obj) {
            if (obj.gameObject.tag == "dead") {
                this._isAlive = false;
            }
        }
    }
}
