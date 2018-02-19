using Unity.Collections;
using UnityEngine;

namespace DanmakU {

/// <summary>
/// A single bullet.
/// </summary>
/// <remarks>
/// Internally implemented as a index and a reference to the pool that owns the Danmaku.
/// Does not actually hold any of the information about the Danmaku: the type is implemented
/// as a value-type struct (to avoid garbage allocation), but the semantics for editing any 
/// property is akin to that of a reference type. All the actual data is stored in the backing
/// DanmakuPool.
/// 
/// As the internal data of the backing DanmakuPool may be moved around after an update, the 
/// internal index of Danmaku may not point to the same logical bullet from update to update.
/// As a result, holding long term stores of Danmaku is ill-advised. Likewise editing Danmaku outside
/// of the scope from which they are read from the pool (i.e. <see cref="Danmaku.Get"> or via enumeration)
/// is equally ill-advised and may lead to undefined behavior.
/// </remarks>
public struct Danmaku {

  public readonly int Id;
  public readonly DanmakuPool Pool;

  internal Danmaku(DanmakuPool pool, int index) {
    Pool = pool;
    Id = index;
  }

  /// <summary>
  /// Gets the number of seconds since the Danmaku was created.
  /// </summary>
  public float Time => Pool.Times[Id];

  public DanmakuState InitialState => Pool.InitialStates[Id];

  /// <summary>
  /// Gets or sets the world position of the Danmaku.
  /// </summary>
  public Vector2 Position {
    get { return Pool.Positions[Id]; }
    set { Pool.Positions[Id] = value; }
  }

  /// <summary>
  /// Gets or sets the rotation of the Danmaku.
  /// </summary>
  /// <remarks>
  /// Units are in radians. 0 points directly right. Pi points directly left.
  /// Controls both the graphical rotation as well as the direction of motion
  /// the Danmaku is moving in.
  /// </remarks>
  public float Rotation {
    get { return Pool.Rotations[Id]; }
    set { Pool.Rotations[Id] = value; }
  }

  /// <summary>
  /// Gets or sets the speed of the Danmaku.
  /// </summary>
  /// <remarks>
  /// Units are in units per second. Movement direction is based on <see cref="Rotation">.
  /// Can be negative.
  /// </remarks>
  public float Speed {
    get { return Pool.Speeds[Id]; }
    set { Pool.Speeds[Id] = value; }
  }

  /// <summary>
  /// Gets or sets the speed of the Danmaku.
  /// </summary>
  /// <remarks>
  /// Units are in radians per second. Can be negative.
  /// </remarks>
  public float AngularSpeed {
    get { return Pool.AngularSpeeds[Id]; }
    set { Pool.AngularSpeeds[Id] = value; }
  }

  /// <summary>
  /// Gets or sets the Danmaku's rendering color.
  /// </summary>
  public Color Color {
    get { return Pool.Colors[Id]; }
    set { Pool.Colors[Id] = value; }
  }

  /// <summary>
  /// Destroys the danmaku object.
  /// </summary>
  /// <remarks>
  /// Calling this funciton simply queues the Danmaku for destruction. It will be
  /// recycled back into it's pool on the pool's next update cycle.
  ///
  /// Destroying the same danmaku more than once or attempting to edit an already
  /// destroyed Danmaku will likely result in undefined behavior.
  /// </remarks>
  public void Destroy() => Pool.Destroy(this);

  public void ApplyState(DanmakuState state) {
    Position = state.Position;
    Rotation = state.Rotation;
    Speed = state.Speed;
    AngularSpeed = state.AngularVelocity;
    Color = state.Color;
  }

}

}