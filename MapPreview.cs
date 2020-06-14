using Godot;
using System;
using nextGame.world;

public class MapPreview : Sprite
{
    private Level Level;

    public override void _Ready()
    {
        Level = GetNode<Level>("/root/Game/Level");

        NoiseTexture noiseTexture = (NoiseTexture) Texture;
        OpenSimplexNoise noise = noiseTexture.Noise;

        noise.Period = Level.MapPeriod;
        noise.Octaves = Level.MapOctaves;
        noise.Persistence = Level.MapPersistence;
        noise.Lacunarity = Level.MapLacunarity;
        noise.Seed = Level.MapSeed;

        noiseTexture.Noise = noise;
        Texture = noiseTexture;
    }
}