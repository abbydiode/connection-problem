using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;
using System.Text.Json;

namespace ConnectionProblem {
    public class Counter : Panel {
        private Label label;
        private WebSocket client = new WebSocket();
        private float score = 0;
        private string message = "WARNING: Connection Problem\nAuto-disconnect in %seconds% seconds";

        public Counter() {
            label = Add.Label(message.Replace("%seconds%", "0.0"), "counter-text");
            InitializeWebsocket();
        }

        public override void Tick() {
            label.Text = $"WARNING: Connection Problem\nAuto-disconnect in {score:0.0} seconds";
            score += RealTime.Delta;
        }

        public async void InitializeWebsocket() {
            client.OnMessageReceived += json => score = JsonSerializer.Deserialize<Score>(json).score;
            client.OnMessageReceived += json => Log.Info($"Received saved score {json}");
            await client.Connect($"ws://connection-problem-server.abbydiode.com/score/{Local.PlayerId}");
        }
    }
}