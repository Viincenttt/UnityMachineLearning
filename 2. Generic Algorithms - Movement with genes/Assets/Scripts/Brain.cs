using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

namespace Assets.Scripts {
    [RequireComponent(typeof(ThirdPersonCharacter))]
    public class Brain : MonoBehaviour {
        private int _dnaLength = 1;
        public float TimeBeforeDeath { get; set; }
        public DNA DNA { get; set; }

        private ThirdPersonCharacter _thirdPersonCharacter;
        private Vector3 _movementVector;
        private bool _isJumping;
        private bool _isAlive;

        public void Init() {
            this.DNA = new DNA(this._dnaLength, 6);
            this._thirdPersonCharacter = this.GetComponent<ThirdPersonCharacter>();
            this.TimeBeforeDeath = 0;
            this._isAlive = true;
        }
	
        private void FixedUpdate () {
            float horizontal = 0;
            float vertical = 0;
            bool crouch = false;
            switch (this.DNA.GetGene(0)) {
                case CharacterAction.Forward: vertical = 1; break;
                case CharacterAction.Back: vertical = -1; break;
                case CharacterAction.Left: horizontal = -1; break;
                case CharacterAction.Right: horizontal = 1; break;
                case CharacterAction.Jump: this._isJumping = true; break;
                case CharacterAction.Crouch: crouch = true; break;
            }

            this._movementVector = vertical * Vector3.forward + horizontal * Vector3.right;
            this._thirdPersonCharacter.Move(this._movementVector, crouch, this._isJumping);
            this._isJumping = false;
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
