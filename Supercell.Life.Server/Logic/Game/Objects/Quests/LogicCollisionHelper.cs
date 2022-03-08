namespace Supercell.Life.Server.Logic.Game.Objects.Quests
{
    using Supercell.Life.Titan.Logic.Math;

    internal class LogicCollisionHelper
    {
        /// <summary>
        /// Checks whether the hero collides with the edge of the map.
        /// </summary>
        internal static bool CollideWithLevel(LogicVector2 vector)
        {
            bool collided = vector.X >= 249 ^ vector.Y >= 249;

            if (collided)
            {
                Debugger.Warning("0 distance to level edge in collideWithLevel");
            }

            return collided;
        }
    }
}
