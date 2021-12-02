namespace Supercell.Life.Server.Logic.Game.Objects.Quests
{
    using Supercell.Life.Titan.Logic;
    using Supercell.Life.Titan.Logic.Json;
    using Supercell.Life.Titan.Logic.Math;

    using Supercell.Life.Server.Logic.Game.Objects.Quests.Items;

    internal class LogicLevel
    {
        internal LogicQuest Quest;

        internal LogicArrayList<Battle> Battles;
        internal int Version;

        internal int CurrentBattle;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicLevel"/> class.
        /// </summary>
        internal LogicLevel(LogicQuest quest, LogicJSONObject json)
        {
            this.Quest = quest;
            this.Battles = new LogicArrayList<Battle>();

            var battlesJson = json.GetJsonArray("battles");
            this.Version = json.GetJsonNumber("ver").GetIntValue();

            for (int i = 0; i < battlesJson.Size; i++)
            {
                this.Battles.Add(new Battle(this, battlesJson.GetJsonObject(i)));
            }
        }

        internal class Battle
        {
            internal LogicLevel Level;

            internal LogicArrayList<Enemy> Enemies;
            internal LogicArrayList<Obstacle> Obstacles;

            internal int BGIndex;
            internal int FGIndex;

            internal int EnemiesKilled;

            /// <summary>
            /// Gets this instance of <see cref="Battle"/> as a <see cref="LogicJSONObject"/>.
            /// </summary>
            internal LogicJSONObject JSON
            {
                get;
            }

            /// <summary>
            /// Gets a value indicating whether this <see cref="Battle"/> is current battle.
            /// </summary>
            internal bool IsCurrentBattle
            {
                get
                {
                    return this.Level.CurrentBattle == this.Level.Battles.IndexOf(this);
                }
            }

            /// <summary>
            /// Gets a value indicating whether this <see cref="Battle"/> is complete.
            /// </summary>
            internal bool IsCompleted
            {
                get
                {
                    return this.Enemies.Size == 0;
                }
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="Battle"/> class.
            /// </summary>
            internal Battle(LogicLevel level, LogicJSONObject battle)
            {
                this.Level = level;
                this.JSON = battle;

                this.InitializeLists();

                this.BGIndex = this.JSON.GetJsonNumber("bg_index").GetIntValue();
                this.FGIndex = this.JSON.GetJsonNumber("fg_index").GetIntValue();
            }

            /// <summary>
            /// Initializes the lists of enemies and obstacles in the battle.
            /// </summary>
            private void InitializeLists()
            {
                this.Enemies = new LogicArrayList<Enemy>();
                this.Obstacles = new LogicArrayList<Obstacle>();

                LogicJSONArray enemies = this.JSON.GetJsonArray("enemies");

                if (enemies != null)
                {
                    for (int i = 0; i < enemies.Size; i++)
                    {
                        this.Enemies.Add(new Enemy(enemies.GetJsonObject(i)));
                    }
                }

                LogicJSONArray obstacles = this.JSON.GetJsonArray("obstacles");

                if (obstacles != null)
                {
                    for (int i = 0; i < obstacles.Size; i++)
                    {
                        this.Obstacles.Add(new Obstacle(obstacles.GetJsonObject(i)));
                    }
                }
            }

            /// <summary>
            /// Checks whether the character at the specified X and Y coordinates collided with an enemy in the battle.
            /// </summary>
            internal void CheckCollision(LogicVector2 vector)
            {
                this.UpdateCharacterPosition(vector);

                if (this.Enemies.Size > 0)
                {
                    for (int i = 0; i < this.Enemies.Size; i++)
                    {
                        Enemy enemy = this.Enemies[i];

                        LogicVector2 enemyVector = new LogicVector2(LogicMath.Cos(enemy.X, 255), LogicMath.Sin(enemy.Y, 255));
                        Debugger.Debug($"Enemy Coords : {enemyVector}, Enemy Angle : {enemyVector.Angle}, Enemy Radius : {LogicMath.GetRadius(enemyVector.X, enemyVector.Y)}, Distance Between : {enemyVector.GetDistance(vector)}");

                        if (this.Level.Quest.SublevelMoveCount == enemy.FirstAttackOnTurn)
                        {
                            this.DamageCharacter(enemy, vector, enemyVector);
                        }
                        else if (this.Level.Quest.SublevelMoveCount > enemy.FirstAttackOnTurn)
                        {
                            if (this.Level.Quest.SublevelMoveCount % enemy.AttackTurnSeq == 0)
                            {
                                this.DamageCharacter(enemy, vector, enemyVector);
                            }
                        }

                        /* if (vector.X == enemyVector.X && vector.Y == enemyVector.Y)
                        {
                            Debugger.Debug(this.Enemies[i].Data.GlobalID);

                            this.Enemies.RemoveAt(i);
                            this.EnemiesKilled++;
                        } */
                    }
                }

                Debugger.Debug($"Current Battle: {this.Level.CurrentBattle}");

                if (this.IsCompleted)
                {
                    this.Level.CurrentBattle++;
                }
            }

            /// <summary>
            /// Updates the character position.
            /// </summary>
            internal void UpdateCharacterPosition(LogicVector2 position)
            {
                this.Level.Quest.Characters[this.Level.Quest.CharacterIndex].Position = position;
            }
            
            /// <summary>
            /// Damages the character.
            /// </summary>
            internal void DamageCharacter(Enemy enemy, LogicVector2 heroVector, LogicVector2 enemyVector)
            {
                if (this.Level.Quest.Characters.Count > 1)
                {
                    int distance = enemyVector.GetDistance(heroVector);

                    if (distance <= enemy.Data.Radius)
                    {
                        LogicCharacter character = this.Level.Quest.Characters.Find(c => c.Position.IsEqual(heroVector));
                        character?.HitCharacter(enemy.Damage);
                    }
                }
                else
                {
                    this.Level.Quest.Characters[0].HitCharacter(enemy.Damage);
                }
            }
        }
    }
}
