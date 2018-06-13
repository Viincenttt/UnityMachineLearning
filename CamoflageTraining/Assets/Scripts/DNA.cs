using UnityEngine;

namespace Assets.Scripts {
    public class DNA : MonoBehaviour {
        [SerializeField] private FloatVariable _elapsedTime;

        public float R { get; set; }
        public float G { get; set; }
        public float B { get; set; }

        public float TimeBeforeDeath { get; set; } = 0f;

        private SpriteRenderer _spriteRenderer;
        private Collider2D _collider2D;
        
        private void Start() {
            this._spriteRenderer = this.GetComponent<SpriteRenderer>();
            this._collider2D = this.GetComponent<Collider2D>();

            this._spriteRenderer.color = new Color(this.R, this.G, this.B);
        }

        private void OnMouseDown() {
            this.TimeBeforeDeath = this._elapsedTime.Value;
            this._spriteRenderer.enabled = false;
            this._collider2D.enabled = false;
        }
    }
}