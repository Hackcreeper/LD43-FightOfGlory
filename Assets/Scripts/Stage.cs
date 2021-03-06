﻿using UnityEngine;

public class Stage
{
    private readonly Arena _arena;

    private readonly string[] _enemies;

    private readonly int _index;

    private readonly Unit[,] _board = new Unit[Arena.FIELD_WIDTH,Arena.FIELD_HEIGHT];

    public Stage(Arena arena, string[] enemies, int index)
    {
        _arena = arena;
        _enemies = enemies;
        _index = index;
    }

    public void Start()
    {
        SpawnPlayerUnits();
        SpawnEnemyUnits();
    }

    private void SpawnPlayerUnits()
    {
        var playerMin = 0;
        var playerMax = Arena.FIELD_WIDTH / 3;

        foreach(var unit in _arena.GetPlayerUnits())
        {
            var x = 0;
            var y = 0;

            do
            {
                x = Random.Range(playerMin, playerMax);
                y = Random.Range(0, Arena.FIELD_HEIGHT);
            } while (_board[x, y] != null);

            unit.transform.position = _arena.FindField(x + 1, y + 1).position;
            unit.SetBoardPosition(x, y);
            _board[x, y] = unit;
        }
    }

    private void SpawnEnemyUnits()
    {
        var enemyMin = Arena.FIELD_WIDTH - (Arena.FIELD_WIDTH / 3);
        var enemyMax = Arena.FIELD_WIDTH - 1;

        foreach(var enemy in _enemies)
        {
            var x = 0;
            var y = 0;

            do
            {
                x = Random.Range(enemyMin, enemyMax);
                y = Random.Range(0, Arena.FIELD_HEIGHT);
            } while (_board[x, y] != null);

            var unit = Object.Instantiate(Resources.Load<GameObject>($"Enemies/{enemy}"));
            unit.transform.position = _arena.FindField(x + 1, y + 1).position;
            unit.transform.rotation = Quaternion.Euler(0, 180, 0);

            var unitComponent = unit.GetComponent<Unit>();
            unitComponent.SetBoardPosition(x, y);

            _board[x, y] = unitComponent;
            _arena.AddEnemy(unitComponent);
        }
    }

    public Unit[,] GetBoard() => _board;

    public void Set(int x, int y, Unit unit)
    {
        _board[x, y] = unit;
    }

    public Unit Get(int x, int y)
    {
        if (x >= Arena.FIELD_WIDTH || y >= Arena.FIELD_HEIGHT || x < 0 || y < 0)
        {
            return null;
        }

        return _board[x, y];
    }

    public int GetIndex() => _index;
}