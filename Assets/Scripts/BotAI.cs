using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class BotAI : MonoBehaviour
{
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private Transform player;
    [SerializeField] private float waitTime = 2f;
    [SerializeField] private float searchTime = 3f;
    [SerializeField] private float patrolSpeed = 3f;
    [SerializeField] private float chaseSpeed = 4f;
    [SerializeField] private float detectionRange = 10f;
    [SerializeField] private float eyeHeight = 1.5f;
    [SerializeField] private float viewAngle = 75f;

    private enum State
    {
        Patrol,
        Chase,
        Search,
        Return
    }

    private State _currentState = State.Patrol;
    private int _currentWaypoint;
    private int _lastWaypoint;
    private float _lostPlayerTimer;
    private float _searchTimer;
    private float _searchDirection = 1f;
    private Vector3 _lastPlayerPosition;
    private bool _isWaiting;
    private bool _hasResearchPosition;

    private NavMeshAgent _agent;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.speed = patrolSpeed;
        GoToNextWaypoint();
    }

    private void Update()
    {
        switch (_currentState)
        {
            case State.Patrol:
                if (player == null) return;

                if (CanSee())
                {
                    _lastWaypoint = _currentWaypoint;
                    _currentState = State.Chase;
                }

                if (_isWaiting) return;
                if (!_agent.pathPending && _agent.remainingDistance < 0.5f)
                {
                    _isWaiting = true;
                    StartCoroutine(WaitAtWaypoint());
                }
                break;

            case State.Chase:
                {
                    _agent.speed = chaseSpeed;

                    if (CanSee())
                    {
                        _lostPlayerTimer = 0f;
                        _lastPlayerPosition = player.position;
                        _agent.SetDestination(player.position);
                    }
                    else
                    {
                        _lostPlayerTimer += Time.deltaTime;
                    }

                    if (_lostPlayerTimer >= 1f)
                    {
                        _hasResearchPosition = false;
                        _currentState = State.Search;
                    }
                }
                break;

            case State.Search:
                _agent.speed = chaseSpeed;
                _agent.SetDestination(_lastPlayerPosition);

                if (CanSee())
                {
                    _lostPlayerTimer = 0f;
                    _currentState = State.Chase;
                }

                if (!_hasResearchPosition && !_agent.pathPending && _agent.remainingDistance < 0.5f)
                {
                    _hasResearchPosition = true;
                    _searchTimer = 0f;
                }
                if (_hasResearchPosition)
                {
                    _searchTimer += Time.deltaTime;
                    _searchDirection = _searchTimer < searchTime / 2f ? 1f : -1f;
                    transform.Rotate(0f, 120f * _searchDirection * Time.deltaTime, 0f);
                }
                if (_searchTimer >= searchTime)
                {
                    _currentState = State.Return;
                }
                break;

            case State.Return:
                _agent.speed = patrolSpeed;

                if (CanSee())
                {
                    _lostPlayerTimer = 0f;
                    _currentState = State.Chase;
                    break;
                }

                _currentWaypoint = _lastWaypoint;
                _agent.SetDestination(waypoints[_currentWaypoint].position);

                if (!_agent.pathPending && _agent.remainingDistance < 0.5f)
                {
                    _currentState = State.Patrol;
                    _isWaiting = true;
                    StartCoroutine(WaitAtWaypoint());
                }
                break;

        }

    }

    private void GoToNextWaypoint()
    {
        if (waypoints.Length == 0) return;

        Transform waypoint = waypoints[_currentWaypoint];
        _agent.SetDestination(waypoint.position);

    }
    private IEnumerator WaitAtWaypoint()
    {
        yield return new WaitForSeconds(waitTime);

        if (_currentState != State.Patrol) yield break;

        _currentWaypoint++;

        if (_currentWaypoint >= waypoints.Length)
        {
            _currentWaypoint = 0;
        }

        _isWaiting = false;

        GoToNextWaypoint();
    }

    private bool CanSee()
    {
        Vector3 eyePosition = transform.position + Vector3.up * eyeHeight;
        Vector3 playerPosition = player.position + Vector3.up * eyeHeight;
        Vector3 directionToPlayer = (playerPosition - eyePosition).normalized;

        float angle = Vector3.Angle(transform.forward, directionToPlayer);

        if (angle > viewAngle)
        {
            return false;
        }

        Debug.DrawRay(eyePosition, directionToPlayer * detectionRange, Color.red);

        if (Physics.Raycast(eyePosition, directionToPlayer, out RaycastHit hit, detectionRange))
        {
            if (hit.transform.CompareTag("Player"))
            {
                return true;
            }
        }
        return false;
    }

}
