using UnityEngine;

public class Gameloop: MonoBehaviour
{
    const float FRAME_TIME = 1f / 60f;
    float accumulator;


    public InputHandler inputHandler1;
    public MovementHandler movementHandler1;

    public AttackHandler attackHandler1;
    public PlayerComponent player1Component;

    public InputHandler inputHandler2;
    public MovementHandler movementHandler2;
    public AttackHandler attackHandler2;
    public PlayerComponent player2Component;

    public PlayerData playerData1;
    public PlayerData playerData2;
    void Awake()
    {

    }


    void OnEnable()
    {
    }


    void OnDisable()
    {
    }


     
    void Update()
    {
        accumulator += Time.deltaTime;

        while (accumulator >= FRAME_TIME)
        {
            SimulateFrame();

            accumulator -= FRAME_TIME;
        }
    }

    void Start()
    {
        player1Component = new PlayerComponent(playerData1,0);
        player2Component = new PlayerComponent(playerData2,1);
    } 


    void UpdatePlayerDirection()
    {
        if (player1Component.CoordX < player2Component.CoordX) {
            player1Component.facing = PlayerComponent.Direction.Right;
            player2Component.facing = PlayerComponent.Direction.Left;
        } else {
            player1Component.facing = PlayerComponent.Direction.Left;
            player2Component.facing = PlayerComponent.Direction.Right;
        }
    }

    void SimulateFrame()
    {
        UpdatePlayerDirection();

        Debug.Log("Player 1 X: " + player1Component.CoordX + " Player 2 X: " + player2Component.CoordX);
        // process inputs        
        FrameInput input1 = inputHandler1.ConsumeInput();
        FrameInput input2 = inputHandler2.ConsumeInput();

        movementHandler1.Tick(input1, 0, player1Component);
        movementHandler2.Tick(input2, 1, player2Component);

        attackHandler1.Tick(input1, 0, player1Component, player2Component);

        attackHandler2.Tick(input2, 1, player2Component, player1Component);

    }
}
