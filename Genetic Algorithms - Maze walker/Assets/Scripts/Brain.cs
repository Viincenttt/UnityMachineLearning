using UnityEngine;

namespace Assets.Scripts {
    public class Brain : MonoBehaviour {
        [SerializeField] private GameObject _eyes;

        public float TimeBeforeDeath { get; set; }
        public DNA DNA { get; set; } 
        public float MaximumDistanceTravelled { get; set; }

        private bool _isAlive;
        private Vector3 _startPosition;

        public void Start() {
            this.TimeBeforeDeath = 0;
            this.MaximumDistanceTravelled = 0;
            this._isAlive = true;
            this._startPosition = this.transform.position;
            this.DNA = new DNA();
        }

        private void Update() {
            if (!this._isAlive) {
                return;
            }

            float distanceTravelled = Vector3.Distance(this.transform.position, this._startPosition);
            if (distanceTravelled > this.MaximumDistanceTravelled) {
                this.MaximumDistanceTravelled = distanceTravelled;
            }
            
            this.TimeBeforeDeath += Time.deltaTime;

            this.MoveForward();
        }

        private void MoveForward() {
            this.transform.Translate(0, 0, 1 * 0.1f);
        }

        private void Rotate() {
            this.transform.Rotate(0, this.DNA.TurningAngle, 0);
        }

        private void OnCollisionEnter(Collision obj) {
            if (obj.gameObject.tag == KnownTags.Dead) {
                this._isAlive = false;
            }
            else if (obj.gameObject.tag == KnownTags.Wall) {
                this.Rotate();
            }
        }
    }
}