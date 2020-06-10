using Godot;
using System;

public class MapPreview : Sprite
{
    private World World;

    public override void _Ready()
    {
        World = GetNode<World>("/root/Game/World");

        NoiseTexture noiseTexture = (NoiseTexture) Texture;
        OpenSimplexNoise noise = noiseTexture.Noise;

        noise.Period = World.MapPeriod;
        noise.Octaves = World.MapOctaves;
        noise.Persistence = World.MapPersistence;
        noise.Lacunarity = World.MapLacunarity;
        noise.Seed = World.MapSeed;

        noiseTexture.Noise = noise;
        Texture = noiseTexture;
    }
}