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
                // TODO: Set a new direction once a hero collides with the level, an obstacle, or an enemy that takes more than one hit to eliminate

                this.UpdateCharacterPosition(vector);
                
                if (LogicCollisionHelper.CollideWithLevel(vector))
                {
                    this.Level.Quest.Characters[this.Level.Quest.CharacterIndex].Bounces++;

                    int newAngle = LogicMath.GetAngle(vector.X, vector.Y);
                    Debugger.Debug(newAngle);
                }

                if (this.Enemies.Size > 0)
                {
                    for (int i = 0; i < this.Enemies.Size; i++)
                    {
                        Enemy enemy = this.Enemies[i];
                        
                        LogicVector2 enemyVector = new LogicVector2(enemy.X / (255 * 255), enemy.Y / (255 * 255));
                        Debugger.Debug($"{enemy.Data.Name} - Coords : {enemyVector}, Angle Between : {enemyVector.GetAngleBetween(vector.X, vector.Y)}, Distance Between : {vector.GetDistance(enemyVector)}");
                        
                        if (this.Level.Quest.SublevelMoveCount == enemy.FirstAttackOnTurn)
                        {
                            foreach (LogicCharacter character in this.Level.Quest.Characters)
                            {
                                this.DamageCharacter(enemy, character.Position, enemyVector);
                            }
                        }
                        else if (this.Level.Quest.SublevelMoveCount > enemy.FirstAttackOnTurn)
                        {
                            if (this.Level.Quest.SublevelMoveCount % enemy.AttackTurnSeq == 0)
                            {
                                foreach (LogicCharacter character in this.Level.Quest.Characters)
                                {
                                    this.DamageCharacter(enemy, character.Position, enemyVector);
                                }
                            }
                        }
                        
                        if (enemyVector.GetAngleBetween(vector.X, vector.Y) <= enemy.Data.Radius + this.Level.Quest.Characters[this.Level.Quest.CharacterIndex].HeroData.Radius)
                        {
                            this.HitEnemy(enemy);
                        }
                    }

                    for (int i = 0; i < this.Obstacles.Size; i++)
                    {
                        Obstacle obstacle = this.Obstacles[i];
                        
                        LogicVector2 obstacleVector = new LogicVector2(obstacle.X / (255 * 255), obstacle.Y / (255 * 255));
                        Debugger.Debug($"{obstacle.Data.Name} - Coords : {obstacleVector}, Angle Between : {obstacleVector.GetAngleBetween(vector.X, vector.Y)}, Distance Between : {vector.GetDistance(obstacleVector)}");
                        
                        if (obstacleVector.GetAngleBetween(vector.X, vector.Y) <= obstacle.Data.Radius)
                        {
                            this.HitObstacle(obstacle);
                        }
                    }
                }
                
                Debugger.Debug($"Current Battle: {this.Level.CurrentBattle}");

                if (this.IsCompleted)
                {
                    this.Level.CurrentBattle++;
                    this.Level.Quest.SetInitialCharacterPositions();
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
            /// Hits the <see cref="Enemy"/> at the specified index.
            /// </summary>
            internal void HitEnemy(Enemy enemy)
            {
                enemy.Hitpoints -= this.Level.Quest.Characters[this.Level.Quest.CharacterIndex].Damage;

                if (enemy.Hitpoints <= 0)
                {
                    Debugger.Debug($"Removed {enemy.Data.Name} ({enemy.Hitpoints + this.Level.Quest.Characters[this.Level.Quest.CharacterIndex].Damage} -> {enemy.Hitpoints})");

                    this.Enemies.Remove(enemy);
                    this.EnemiesKilled++;
                }
            }

            /// <summary>
            /// Hits the <see cref="Obstacle"/> at the specified index.
            /// </summary>
            internal void HitObstacle(Obstacle obstacle)
            {
                obstacle.Hits++;
                this.Level.Quest.Characters[this.Level.Quest.CharacterIndex].Bounces++;

                if (obstacle.Data.GlobalID == 15000001) // Beehive
                {
                    switch (obstacle.Hits)
                    {
                        case 1:
                        {
                            // this.Enemies.Add(new Enemy(12000002, obstacle.X / (255 * 255) - 5, obstacle.Y / (255 * 255)));
                            break;
                        }
                        case 2:
                        {
                            // this.Enemies.Add(new Enemy(12000002, obstacle.X / (255 * 255), obstacle.Y / (255 * 255) + 5));
                            break;
                        }
                        case 3:
                        {
                            // this.Enemies.Add(new Enemy(12000002, obstacle.X / (255 * 255), obstacle.Y / (255 * 255) - 5));
                            break;
                        }
                    }
                }

                if (obstacle.Hits == obstacle.Data.Hitpoints)
                {
                    Debugger.Debug($"Removed {obstacle.Data.Name}");
                    this.Obstacles.Remove(obstacle);
                }
            }

            /// <summary>
            /// Damages the character.
            /// </summary>
            internal void DamageCharacter(Enemy enemy, LogicVector2 heroVector, LogicVector2 enemyVector)
            {
                int angle = enemyVector.GetAngleBetween(heroVector.X, heroVector.Y);

                if (angle <= enemy.Data.Radius)
                {
                    LogicCharacter character = this.Level.Quest.Characters.Find(c => c.Position.IsEqual(heroVector));
                    character?.HitCharacter(enemy.Damage);
                }
            }
        }
    }
}
