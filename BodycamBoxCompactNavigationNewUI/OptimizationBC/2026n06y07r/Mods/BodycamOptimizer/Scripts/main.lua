----------------------------------------------------------
-- Configuration
----------------------------------------------------------
-- Default graphics preset
-- Available:
-- "Vanilla Graphics"
-- "Balanced"
-- "Performance"
-- "Ultra Performance"
local Mode = "Ultra Performance"

-- Automatically apply the preset after entering the game
local AutoApply = true

-- Delay (seconds) before applying graphics settings
local AutoApplyDelay = 6

-- Wait after Bodycam starts (milliseconds)
local AutoHotkeyStartupDelay = 7000

-- Interval between the two presses (milliseconds)
local AutoHotkeyPressDelay = 500


------------------------------------------------------------------------------------------------------------------------------
--  开发者：   QQ 1598395898    / QQ 672095630
--  授权声明： 允许个人学习、修改与二次扩展。
--  版权限制： 未经允许禁止用于任何商业牟利项目。署名权归原作者所有。
------------------------------------------------------------------------------------------------------------------------------


----------------------------------------------------------
-- Auto Hotkey
--
-- Automatically triggers a graphics preset after startup.
-- This performs the same action as pressing the hotkey.
----------------------------------------------------------
local AutoHotkeyMode = "Ultra Performance"

-- Available Modes:
--
-- "None"
-- "Balanced"
-- "Performance"
-- "Ultra Performance"


----------------------------------------------------------
-- Unreal Engine Helpers
----------------------------------------------------------
local UEHelpers = require("UEHelpers")

local GetWorld = UEHelpers.GetWorld  --GetKismetSystemLibrary()
local GetKismetSystemLibrary = UEHelpers.GetKismetSystemLibrary


----------------------------------------------------------
-- Console Command Wrapper
----------------------------------------------------------
local function ExecCmd(Command, Value)

    local FullCommand = Command .. " " .. tostring(Value)

    ExecuteInGameThread(function()

        local KismetSystemLibrary = GetKismetSystemLibrary()
        local Engine = FindFirstOf("Engine")

        if KismetSystemLibrary
        and KismetSystemLibrary:IsValid()
        and Engine
        and Engine:IsValid() then

            KismetSystemLibrary:ExecuteConsoleCommand(
                Engine,
                FullCommand,
                nil
            )

        end

    end)

end


----------------------------------------------------------
-- Runtime State
----------------------------------------------------------
-- Current loaded map
local CurrentMap = nil

-- Current map override
local CurrentMapOverride = nil

-- Current graphics preset
local CurrentPreset = nil

-- Initialization state
local IsInitialized = false

-- Delayed task queue
local TaskQueue = {}

-- Last level load timestamp
local LastLevelLoad = 0

-- Pending map reload
local PendingMapReload = false


----------------------------------------------------------
-- Preset Index
----------------------------------------------------------
local PresetIndex = {

    ["Vanilla Graphics"] = 1,

    ["Balanced"] = 2,

    ["Performance"] = 3,

    ["Ultra Performance"] = 4,

}


----------------------------------------------------------
-- Graphics Presets
----------------------------------------------------------
------------- # PRESET 1 / 4 # -------------
--
-- Vainilla Graphics  (Original)
local function ApplyVanilla() -- point-1

    ----------------------------------------------------------
    -- Shadows
    ----------------------------------------------------------    
    ExecCmd("r.ShadowQuality", 5) --3                         -- Overall shadow quality
    ExecCmd("r.Shadow.DistanceScale", 1)  -- 0.16             -- Shadow render distance
    ExecCmd("r.Shadow.CSM.MaxCascades", 4)                -- Cascaded shadow levels
    ExecCmd("r.Shadow.MaxCSMResolution", 1024)  -- 1024    -- Shadow map resolution
    ExecCmd("r.Shadow.RadiusThreshold", 0.05)             -- Shadow visibility threshold
    ExecCmd("r.Shadow.Denoiser", 2)                       -- Shadow denoiser
    ExecCmd("r.Shadow.Virtual.Enable", 1)                 -- Virtual Shadow Maps
    ExecCmd("r.Shadow.Virtual.SMRT.RayCountLocal", 8)     -- Local shadow ray count
    ExecCmd("r.Shadow.Virtual.SMRT.RayCountDirectional", 4) -- Directional shadow ray count
    ExecCmd("r.Shadow.Virtual.Clipmap.FirstLevel", 6)     -- First virtual shadow clipmap level
    ExecCmd("r.Shadow.Virtual.Clipmap.FirstCoarseLevel", 15) -- First coarse clipmap level
    ExecCmd("r.Shadow.NaniteLODBias", 1)                  -- Nanite shadow LOD bias    

    ----------------------------------------------------------
    -- Nanite
    ----------------------------------------------------------
    ExecCmd("r.Nanite", 1)                               -- Enable Nanite rendering
    ExecCmd("r.Nanite.ViewMeshLODBias.Offset", 0)        -- Nanite mesh LOD bias
    ExecCmd("r.Nanite.MinPixelsPerEdgeHW", 32)           -- Minimum pixels per edge
    ExecCmd("r.Nanite.ShadeBinningMode", 0)              -- Nanite shading mode

    ----------------------------------------------------------
    -- Foliage （植被）
    ----------------------------------------------------------
    ExecCmd("foliage.OnlyLOD", -1)                      -- Force a specific foliage LOD
    ExecCmd("foliage.DitheredLOD", 1)                   -- Enable smooth LOD transitions
    ExecCmd("foliage.LODDistanceScale", 1)              -- Foliage LOD distance scale
    ExecCmd("foliage.MinimumScreenSize", 0.000005)      -- Minimum foliage screen size

    ----------------------------------------------------------
    -- Lumen
    ----------------------------------------------------------
    ExecCmd("r.Lumen.Reflections.Allow", 1)                           -- Enable Lumen reflections
    ExecCmd("r.Lumen.ScreenProbeGather.DownsampleFactor", 32)         -- Screen probe downsample factor
    ExecCmd("r.Lumen.ScreenProbeGather.ShortRangeAO", 1)              -- Short-range ambient occlusion
    ExecCmd("r.Lumen.ScreenProbeGather.Temporal.MaxFramesAccumulated", 10) -- Temporal accumulation frames
    ExecCmd("r.Lumen.HardwareRayTracing", 1)                          -- Hardware ray tracing

    ----------------------------------------------------------
    -- Reflections
    ----------------------------------------------------------
    ExecCmd("r.SSR.Quality", 3)                     -- Screen Space Reflection quality

    ----------------------------------------------------------
    -- View Distance
    ----------------------------------------------------------
    ExecCmd("r.StaticMeshLODDistanceScale", 1)           -- Static mesh LOD distance scale

    ----------------------------------------------------------
    -- Post Processing
    ----------------------------------------------------------
    ExecCmd("r.DepthOfFieldQuality", 2)                 -- Depth of field quality
    ExecCmd("r.Tonemapper.Sharpen", 4)                  -- Image sharpening
    --ExecCmd("r.PostProcessing.DisableMaterials", 0)     -- Disable post-process materials
    ExecCmd("r.EyeAdaptationQuality", 1)                -- Eye adaptation quality

    ----------------------------------------------------------
    -- Effects
    ----------------------------------------------------------
    ExecCmd("r.VolumetricCloud", 1)              -- Volumetric cloud rendering
    ExecCmd("r.LightShaftQuality", 1)            -- Light shaft quality
    ExecCmd("r.LightFunctionQuality", 2)         -- Light function quality

    ----------------------------------------------------------
    -- 添加 / 自定义
    ----------------------------------------------------------
    --不知道
    ExecCmd("foliage.DensityScale", 1.0) 
    --降低整个画质来提升帧率
    ExecCmd("r.ScreenPercentage",100)

end

------------- # PRESET 2 / 4 # -------------

-- Balanced

-- Best balance between image quality and performance.

-- Recommended:
-- RTX 2060+
-- GTX 1660
-- RTX 3050

-- Average FPS Gain:
-- +20~35%
local function ApplyBalanced() --2

    ----------------------------------------------------------
    -- Shadows
    ----------------------------------------------------------    
    ExecCmd("r.ShadowQuality",4)    -- Overall shadow quality
    ExecCmd("r.Shadow.DistanceScale",0.64)  -- Shadow render distance
    ExecCmd("r.Shadow.CSM.MaxCascades", 1)  -- Cascaded shadow levels
    ExecCmd("r.Shadow.MaxCSMResolution", 8192)             -- Shadow map resolution
    ExecCmd("r.Shadow.RadiusThreshold", 0.03)              -- Shadow visibility threshold
    ExecCmd("r.Shadow.Denoiser", 0)                        -- Shadow denoiser
    ExecCmd("r.Shadow.Virtual.Enable", 0)                  -- Virtual Shadow Maps
    ExecCmd("r.Shadow.Virtual.SMRT.RayCountLocal", 0)      -- Local shadow ray count
    ExecCmd("r.Shadow.Virtual.SMRT.RayCountDirectional", 0)-- Directional shadow ray count
    ExecCmd("r.Shadow.Virtual.Clipmap.FirstLevel", 6)      -- First virtual shadow clipmap level
    ExecCmd("r.Shadow.Virtual.Clipmap.FirstCoarseLevel", 32) -- First coarse clipmap level
    ExecCmd("r.Shadow.NaniteLODBias", 3)                   -- Nanite shadow LOD bias

    ----------------------------------------------------------
    -- Nanite
    ----------------------------------------------------------
    ExecCmd("r.Nanite", 0)                               -- Enable Nanite rendering
    ExecCmd("r.Nanite.ViewMeshLODBias.Offset", 2)        -- Nanite mesh LOD bias
    ExecCmd("r.Nanite.MinPixelsPerEdgeHW", 0)            -- Minimum pixels per edge
    ExecCmd("r.Nanite.ShadeBinningMode", 1)              -- Nanite shading mode

    ----------------------------------------------------------
    -- Foliage（植被）
    ----------------------------------------------------------
    ExecCmd("foliage.OnlyLOD", 0)                       -- Force a specific foliage LOD
    ExecCmd("foliage.DitheredLOD", 1)                   -- Enable smooth LOD transitions
    ExecCmd("foliage.LODDistanceScale", 2)              -- Foliage LOD distance scale
    ExecCmd("foliage.MinimumScreenSize", 0.000005)      -- Minimum foliage screen size

    ----------------------------------------------------------
    -- Lumen
    ----------------------------------------------------------
    ExecCmd("r.Lumen.Reflections.Allow", 0)                           -- Enable Lumen reflections
    ExecCmd("r.Lumen.ScreenProbeGather.DownsampleFactor", 32)         -- Screen probe downsample factor
    ExecCmd("r.Lumen.ScreenProbeGather.ShortRangeAO", 0)              -- Short-range ambient occlusion
    ExecCmd("r.Lumen.ScreenProbeGather.Temporal.MaxFramesAccumulated", 32) -- Temporal accumulation frames
    ExecCmd("r.Lumen.HardwareRayTracing", 0)                          -- Hardware ray tracing

    ----------------------------------------------------------
    -- Reflections
    ----------------------------------------------------------
    ExecCmd("r.SSR.Quality", 1)                     -- Screen Space Reflection quality

    ----------------------------------------------------------
    -- View Distance
    ----------------------------------------------------------
    ExecCmd("r.StaticMeshLODDistanceScale", 0.25)        -- Static mesh LOD distance scale

    ----------------------------------------------------------
    -- Post Processing
    ----------------------------------------------------------
    ExecCmd("r.DepthOfFieldQuality", 0)                 -- Depth of field quality
    ExecCmd("r.Tonemapper.Sharpen", 4)                  -- Image sharpening
    --ExecCmd("r.PostProcessing.DisableMaterials", 0)     -- Disable post-process materials
    ExecCmd("r.EyeAdaptationQuality", 1)                -- Eye adaptation quality

    ----------------------------------------------------------
    -- Effects
    ----------------------------------------------------------
    ExecCmd("r.VolumetricCloud", 0)              -- Volumetric cloud rendering
    ExecCmd("r.LightShaftQuality", 0)            -- Light shaft quality
    ExecCmd("r.LightFunctionQuality", 2)         -- Light function quality

    ----------------------------------------------------------
    -- 添加 / 自定义
    ----------------------------------------------------------
    --不知道
    ExecCmd("foliage.DensityScale", 1.0) 
    --降低整个画质来提升帧率
    ExecCmd("r.ScreenPercentage", 95)

end

------------- # PRESET 3 / 4 # -------------

-- Performance

-- Prioritizes frame rate over visual quality.

-- Recommended:
-- GTX 1060
-- GTX 1650

-- Average FPS Gain:
-- +35~50%
local function ApplyPerformance() --3

    ----------------------------------------------------------
    -- Shadows
    ----------------------------------------------------------    
    ExecCmd("r.ShadowQuality", 3)                          -- Overall shadow quality
    ExecCmd("r.Shadow.DistanceScale", 0.64)               -- Shadow render distance
    ExecCmd("r.Shadow.CSM.MaxCascades", 1)                -- Cascaded shadow levels
    ExecCmd("r.Shadow.MaxCSMResolution", 8192)            -- Shadow map resolution
    ExecCmd("r.Shadow.RadiusThreshold", 0.03)             -- Shadow visibility threshold
    ExecCmd("r.Shadow.Denoiser", 0)                       -- Shadow denoiser
    ExecCmd("r.Shadow.Virtual.Enable", 0)                 -- Virtual Shadow Maps
    ExecCmd("r.Shadow.Virtual.SMRT.RayCountLocal", 0)     -- Local shadow ray count
    ExecCmd("r.Shadow.Virtual.SMRT.RayCountDirectional", 0) -- Directional shadow ray count
    ExecCmd("r.Shadow.Virtual.Clipmap.FirstLevel", 8)     -- First virtual shadow clipmap level
    ExecCmd("r.Shadow.Virtual.Clipmap.FirstCoarseLevel", 32) -- First coarse clipmap level
    ExecCmd("r.Shadow.NaniteLODBias", 3)                  -- Nanite shadow LOD bias    

    ----------------------------------------------------------
    -- Nanite
    ----------------------------------------------------------
    ExecCmd("r.Nanite", 0)                               -- Enable Nanite rendering
    ExecCmd("r.Nanite.ViewMeshLODBias.Offset", 3)        -- Nanite mesh LOD bias
    ExecCmd("r.Nanite.MinPixelsPerEdgeHW", 0)            -- Minimum pixels per edge
    ExecCmd("r.Nanite.ShadeBinningMode", 1)              -- Nanite shading mode

    ----------------------------------------------------------
    -- Foliage（植被）
    ----------------------------------------------------------
    ExecCmd("foliage.OnlyLOD", 0)                       -- Force a specific foliage LOD
    ExecCmd("foliage.DitheredLOD", 1)                   -- Enable smooth LOD transitions
    ExecCmd("foliage.LODDistanceScale", 2)              -- Foliage LOD distance scale
    ExecCmd("foliage.MinimumScreenSize", 0.000005)      -- Minimum foliage screen size

    ----------------------------------------------------------
    -- Lumen
    ----------------------------------------------------------
    ExecCmd("r.Lumen.Reflections.Allow", 0)                           -- Enable Lumen reflections
    ExecCmd("r.Lumen.ScreenProbeGather.DownsampleFactor", 32)         -- Screen probe downsample factor
    ExecCmd("r.Lumen.ScreenProbeGather.ShortRangeAO", 0)              -- Short-range ambient occlusion
    ExecCmd("r.Lumen.ScreenProbeGather.Temporal.MaxFramesAccumulated", 32) -- Temporal accumulation frames
    ExecCmd("r.Lumen.HardwareRayTracing", 0)                          -- Hardware ray tracing

    ----------------------------------------------------------
    -- Reflections
    ----------------------------------------------------------
    ExecCmd("r.SSR.Quality", 1)                     -- Screen Space Reflection quality

    ----------------------------------------------------------
    -- View Distance
    ----------------------------------------------------------
    ExecCmd("r.StaticMeshLODDistanceScale", 0.25)           -- Static mesh LOD distance scale

    ----------------------------------------------------------
    -- Post Processing
    ----------------------------------------------------------
    ExecCmd("r.DepthOfFieldQuality", 0)                 -- Depth of field quality
    ExecCmd("r.Tonemapper.Sharpen", 4)                  -- Image sharpening
    --ExecCmd("r.PostProcessing.DisableMaterials", 1)     -- Disable post-process materials
    ExecCmd("r.EyeAdaptationQuality", 1)                -- Eye adaptation quality

    ----------------------------------------------------------
    -- Effects
    ----------------------------------------------------------
    ExecCmd("r.VolumetricCloud", 0)              -- Volumetric cloud rendering
    ExecCmd("r.LightShaftQuality", 0)            -- Light shaft quality
    ExecCmd("r.LightFunctionQuality", 2)         -- Light function quality

    ----------------------------------------------------------
    -- 添加 / 自定义
    ----------------------------------------------------------
    --不知道
    ExecCmd("foliage.DensityScale", 1.0) 
    --降低整个画质来提升帧率
    ExecCmd("r.ScreenPercentage",90)

end 

------------- # PRESET 4 / 4 # -------------

-- Ultra Performance

-- Maximum FPS for older hardware.

-- Recommended:
-- GTX 970
-- GTX 1050 Ti
-- GTX 1060 3GB
-- GTX 1650

-- Average FPS Gain:
-- +50~70%
local function ApplyUltraPerformance() --4

    ----------------------------------------------------------
    -- Shadows
    ----------------------------------------------------------    
    ExecCmd("r.ShadowQuality", 3)                          -- Overall shadow quality  默认是0，但要用4
    ExecCmd("r.Shadow.DistanceScale", 0.64)               -- Shadow render distance    默认是0，用0.50
    ExecCmd("r.Shadow.CSM.MaxCascades", 1)                -- Cascaded shadow levels 默认是0
    ExecCmd("r.Shadow.MaxCSMResolution", 4096)            -- Shadow map resolution 默认是1024
    ExecCmd("r.Shadow.RadiusThreshold", 0.03)             -- Shadow visibility threshold 默认是0
    ExecCmd("r.Shadow.Denoiser", 0)                       -- Shadow denoiser
    ExecCmd("r.Shadow.Virtual.Enable", 0)                 -- Virtual Shadow Maps
    ExecCmd("r.Shadow.Virtual.SMRT.RayCountLocal", 0)     -- Local shadow ray count
    ExecCmd("r.Shadow.Virtual.SMRT.RayCountDirectional", 0) -- Directional shadow ray count
    ExecCmd("r.Shadow.Virtual.Clipmap.FirstLevel", 8)     -- First virtual shadow clipmap level --
    ExecCmd("r.Shadow.Virtual.Clipmap.FirstCoarseLevel", 32) -- First coarse clipmap level
    ExecCmd("r.Shadow.NaniteLODBias", 3)                  -- Nanite shadow LOD bias 默认是0 
    ----------------------------------------------------------
    -- Nanite
    ----------------------------------------------------------
    ExecCmd("r.Nanite", 0)                               -- Enable Nanite rendering
    ExecCmd("r.Nanite.ViewMeshLODBias.Offset", 3)        -- Nanite mesh LOD bias
    ExecCmd("r.Nanite.MinPixelsPerEdgeHW", 0)            -- Minimum pixels per edge
    ExecCmd("r.Nanite.ShadeBinningMode", 1)              -- Nanite shading mode
        
    ----------------------------------------------------------
    -- Foliage（植被）
    ----------------------------------------------------------
    ExecCmd("foliage.OnlyLOD", 0)                       -- Force a specific foliage LOD
    ExecCmd("foliage.DitheredLOD", 1)                   -- Enable smooth LOD transitions
    ExecCmd("foliage.LODDistanceScale", 2)              -- Foliage LOD distance scale
    ExecCmd("foliage.MinimumScreenSize", 0.000005)      -- Minimum foliage screen size

    ----------------------------------------------------------
    -- Lumen
    ----------------------------------------------------------
    ExecCmd("r.Lumen.Reflections.Allow", 0)                           -- Enable Lumen reflections
    ExecCmd("r.Lumen.ScreenProbeGather.DownsampleFactor", 32)         -- Screen probe downsample factor
    ExecCmd("r.Lumen.ScreenProbeGather.ShortRangeAO", 0)              -- Short-range ambient occlusion
    ExecCmd("r.Lumen.ScreenProbeGather.Temporal.MaxFramesAccumulated", 32) -- Temporal accumulation frames --默认32
    ExecCmd("r.Lumen.HardwareRayTracing", 0)                          -- Hardware ray tracing

    ----------------------------------------------------------
    -- Reflections
    ----------------------------------------------------------
    ExecCmd("r.SSR.Quality", 1)                     -- Screen Space Reflection quality

    ----------------------------------------------------------
    -- View Distance
    ----------------------------------------------------------
    ExecCmd("r.StaticMeshLODDistanceScale", 0.25)           -- Static mesh LOD distance scale 默认是1

    ----------------------------------------------------------
    -- Post Processing
    ----------------------------------------------------------
    ExecCmd("r.DepthOfFieldQuality", 0)                 -- Depth of field quality
    ExecCmd("r.Tonemapper.Sharpen", 4)                  -- Image sharpening
    --ExecCmd("r.PostProcessing.DisableMaterials", 1)     -- Disable post-process materials
    ExecCmd("r.EyeAdaptationQuality", 1)                -- Eye adaptation quality

    ----------------------------------------------------------
    -- Effects
    ----------------------------------------------------------
    ExecCmd("r.VolumetricCloud", 0)              -- Volumetric cloud rendering
    ExecCmd("r.LightShaftQuality", 0)            -- Light shaft quality
    ExecCmd("r.LightFunctionQuality", 2)         -- Light function quality 默认2

    ----------------------------------------------------------
    -- 添加 / 自定义
    ----------------------------------------------------------
    --不知道
    ExecCmd("foliage.DensityScale", 1.0) 
    --降低整个画质来提升帧率
    ExecCmd("r.ScreenPercentage",85)

end

----------------------------------------------------------
-- Map Overrides
----------------------------------------------------------
local MapOverrides = {}

----------------------------------------------------------
-- Lobby
----------------------------------------------------------
MapOverrides["Level /Game/Map/Lobby/Lobby.Lobby:PersistentLevel"] = {

    ["Vanilla Graphics"] = {

        ["r.ShadowQuality"] = 2, --5
        ["r.EyeAdaptationQuality"] = 1,
        ["r.Shadow.MaxCSMResolution"] = 1024,
        ["r.Shadow.DistanceScale"] = 1.0,
        ["r.VolumetricCloud"] = 1,
        --up fps 1
        ["r.ScreenPercentage"] = 100,

    },

    ["Balanced"] = {

        ["r.ShadowQuality"] = 2,
        ["r.EyeAdaptationQuality"] = 1,
        ["r.Shadow.MaxCSMResolution"] = 4096,
        ["r.Shadow.DistanceScale"] = 1.0,
        ["r.VolumetricCloud"] = 1,
        --up fps 1
        ["r.ScreenPercentage"] = 75,

    },

    ["Performance"] = {

        ["r.ShadowQuality"] = 2,
        ["r.EyeAdaptationQuality"] = 1,
        ["r.Shadow.MaxCSMResolution"] = 4096,
        ["r.Shadow.DistanceScale"] = 1.0,
        ["r.VolumetricCloud"] = 1,
        --up fps 1
        ["r.ScreenPercentage"] = 75,

    },

    ["Ultra Performance"] = {

        ["r.ShadowQuality"] = 2, --4
        ["r.EyeAdaptationQuality"] = 1,
        ["r.Shadow.MaxCSMResolution"] = 4096,
        ["r.Shadow.DistanceScale"] = 0.16,
        ["r.VolumetricCloud"] = 0,
        --up fps 1
        ["r.ScreenPercentage"] = 75,

    },

}

----------------------------------------------------------
-- CQB
----------------------------------------------------------
MapOverrides["Level /Game/Map/CQB/CQB.CQB:PersistentLevel"] = {

    ["Vanilla Graphics"] = {

        ["r.LightFunctionQuality"] = 2,
        ["r.VolumetricCloud"] = 1,

        --path  article 1
        ["foliage.OnlyLOD"] = -1,
        --path resolution 2
        ["foliage.DensityScale"] = 1.0,
        --path recover article distance 3
        ["foliage.LODDistanceScale"] = 1,
        --low resolition +fps 4
        ["r.ScreenPercentage"] = 100,
        --path xaa 5
        ["r.Shadow.Virtual.Enable"] = 1,
        -- up fps 6
        ["r.ShadowQuality"] = 5,

    },

    ["Balanced"] = {

        ["r.LightFunctionQuality"] = 2,
        ["r.VolumetricCloud"] = 1,

        --path article 1
        ["foliage.OnlyLOD"] = -1,
        --path resolution 2
        ["foliage.DensityScale"] = 0,
        --path recover article distance 3
        ["foliage.LODDistanceScale"] = 0.8,
        --low resolition +fps 4
        ["r.ScreenPercentage"] = 80,
        --path xaa 5
        ["r.Shadow.Virtual.Enable"] = 1,
        -- up fps 6
        ["r.ShadowQuality"] = 4,

    },

    ["Performance"] = {

        ["r.LightFunctionQuality"] = 2,
        ["r.VolumetricCloud"] = 0,

        --path article 1
        ["foliage.OnlyLOD"] = -1,
        --path resolution 2
        ["foliage.DensityScale"] = 0,
        --path recover article distance 3
        ["foliage.LODDistanceScale"] = 0.5,
        --low resolition +fps 4
        ["r.ScreenPercentage"] = 70,
        --path xaa 5
        ["r.Shadow.Virtual.Enable"] = 1,
        -- up fps 6
        ["r.ShadowQuality"] = 2,

    },

    ["Ultra Performance"] = {

        ["r.LightFunctionQuality"] = 2,
        ["r.VolumetricCloud"] = 0,

        --path article 1
        ["foliage.OnlyLOD"] = 0,
        --path resolution 2
        ["foliage.DensityScale"] = 0,
        --path recover article distance 3
        ["foliage.LODDistanceScale"] = 0.2,
        --low resolition +fps 4
        ["r.ScreenPercentage"] = 65,
        --path xaa 5
        ["r.Shadow.Virtual.Enable"] = 1,
        -- up fps 6
        ["r.ShadowQuality"] = 2,

    },

}

----------------------------------------------------------
-- Russian-Building
----------------------------------------------------------
MapOverrides["Level /Game/Map/RussianBuilding/RussianBuilding.RussianBuilding:PersistentLevel"] = {

    ["Vanilla Graphics"] = {

        ["r.Lumen.HardwareRayTracing"] = 1,  
        ["r.Shadow.DistanceScale"] = 0.16,  --增加远距离性
        ["r.Shadow.MaxCSMResolution"] = 1024, --防止透光     

    },

    ["Balanced"] = {

        ["r.Lumen.HardwareRayTracing"] = 1,
        ["r.Shadow.DistanceScale"] = 0.64,  --增加远距离性
        ["r.Shadow.MaxCSMResolution"] = 8192, --防止透光     

    },

    ["Performance"] = {

        ["r.Lumen.HardwareRayTracing"] = 1,
        ["r.Shadow.DistanceScale"] = 0.64,  --增加远距离性
        ["r.Shadow.MaxCSMResolution"] = 8192, --防止透光     

    },

    ["Ultra Performance"] = {

        ["r.Lumen.HardwareRayTracing"] = 1,
        ["r.Shadow.DistanceScale"] = 0.64,  --增加远距离性
        ["r.Shadow.MaxCSMResolution"] = 8192, --防止透光     
    },  --只改了颜色改善

}

----------------------------------------------------------
-- Tumblewood
----------------------------------------------------------
MapOverrides["Level /Game/Map/Tumblewood/Tumblewood.Tumblewood:PersistentLevel"] = {

    ["Vanilla Graphics"] = {

        ["foliage.OnlyLOD"] = -1,
        ["foliage.DitheredLOD"] = 1,
        ["foliage.LODDistanceScale"] = 1,
        ["foliage.MinimumScreenSize"] = 0.000005,
        ["r.Shadow.CSM.MaxCascades"] = 3,
        ["r.Lumen.HardwareRayTracing"] = 1,

    },

    ["Balanced"] = {

        ["foliage.OnlyLOD"] = -1,
        ["foliage.DitheredLOD"] = 0,
        ["foliage.LODDistanceScale"] = 2,
        ["foliage.MinimumScreenSize"] = 0.1,
        ["r.Shadow.CSM.MaxCascades"] = 0,
        ["r.Lumen.HardwareRayTracing"] = 1,

    },

    ["Performance"] = {

        ["foliage.OnlyLOD"] = -1,
        ["foliage.DitheredLOD"] = 0,
        ["foliage.LODDistanceScale"] = 2,
        ["foliage.MinimumScreenSize"] = 0.1,
        ["r.Shadow.CSM.MaxCascades"] = 0,
        ["r.Lumen.HardwareRayTracing"] = 0,

    },

    ["Ultra Performance"] = {

        ["foliage.OnlyLOD"] = 0,
        ["foliage.DitheredLOD"] = 0,
        ["foliage.LODDistanceScale"] = 1,
        ["foliage.MinimumScreenSize"] = 0.1,
        ["r.Shadow.CSM.MaxCascades"] = 0,
        ["r.Lumen.HardwareRayTracing"] = 0,

    },

}

----------------------------------------------------------
-- BombHouse
----------------------------------------------------------
MapOverrides["Level /Game/Map/BombHouse/BombHouse.BombHouse:PersistentLevel"] = {

    ["Vanilla Graphics"] = {

        ["r.EyeAdaptationQuality"] = 1,

    },

    ["Balanced"] = {

        ["r.EyeAdaptationQuality"] = 1,

    },

    ["Performance"] = {

        ["r.EyeAdaptationQuality"] = 1,

    },

    ["Ultra Performance"] = {

        ["r.EyeAdaptationQuality"] = 1, --0

    },

}

----------------------------------------------------------
-- Paintball
----------------------------------------------------------
MapOverrides["Level /Game/Map/Paintball/Paintball.Paintball:PersistentLevel"] = {

    ["Vanilla Graphics"] = {

        ["r.EyeAdaptationQuality"] = 1,
        --patch
        ["r.Shadow.DistanceScale"] = 0.16,  --增加远距离性
        ["r.Shadow.MaxCSMResolution"] = 1024, --防止透光
        ["r.SSR.Quality"] = 3, --反射质量
        
    },

    ["Balanced"] = {

        ["r.EyeAdaptationQuality"] = 1,
        --patch
        ["r.Shadow.DistanceScale"] = 0.64,  --增加远距离性
        ["r.Shadow.MaxCSMResolution"] = 8192, --防止透光
        ["r.SSR.Quality"] = 3, --反射质量
        
    },

    ["Performance"] = {

        ["r.EyeAdaptationQuality"] = 1,
        --patch
        ["r.Shadow.DistanceScale"] = 0.64,  --增加远距离性
        ["r.Shadow.MaxCSMResolution"] = 8192, --防止透光
        ["r.SSR.Quality"] = 3, --反射质量
    
    },

    ["Ultra Performance"] = {

        ["r.EyeAdaptationQuality"] = 1,
        --patch
        ["r.Shadow.DistanceScale"] = 0.64,  --增加远距离性
        ["r.Shadow.MaxCSMResolution"] = 4096, --防止透光
        ["r.SSR.Quality"] = 3, --反射质量

    },

}

----------------------------------------------------------
-- Asylum    这个文件夹跟其他不一样注意！   
----------------------------------------------------------
MapOverrides["Level /Game/Asylum/Maps/Asylum.Asylum:PersistentLevel"] = {

    ["Vanilla Graphics"] = {

        --low resolition +fps 1
        ["r.ScreenPercentage"] = 100,
        --realist zombie hospital crazy 2 
        ["r.ShadowQuality"] = 5,
        ["r.Shadow.DistanceScale"] = 1,

    },

    ["Balanced"] = {

        --low resolition +fps 1
        ["r.ScreenPercentage"] = 90,
        --realist shadow 2 
        ["r.ShadowQuality"] = 4,
        ["r.Shadow.DistanceScale"] = 0.75,

    },

    ["Performance"] = {

        --low resolition +fps 1
        ["r.ScreenPercentage"] = 80,
        --realist shadow 2 
        ["r.ShadowQuality"] = 4,
        ["r.Shadow.DistanceScale"] = 0.75,

    },

    ["Ultra Performance"] = {

        --low resolition +fps 1
        ["r.ScreenPercentage"] = 70,
        --realist shadow 2 
        ["r.ShadowQuality"] = 4,
        ["r.Shadow.DistanceScale"] = 0.75,
        
    },

}

----------------------------------------------------------
-- Village
----------------------------------------------------------
MapOverrides["Level /Game/Map/Village/Village.Village:PersistentLevel"] = {

    ["Vanilla Graphics"] = {

        ["foliage.OnlyLOD"] = -1,

        --path resolution 1
        ["foliage.DensityScale"] = 1.0,
        --path recover article distance 2
        ["foliage.LODDistanceScale"] = 1,
        --low resolition +fps 3
        ["r.ScreenPercentage"] = 100,
        --up fps 4
        ["foliage.DensityScale"] = 1,
        --relist ear 5
        ["r.Shadow.DistanceScale"] = 1,
        --realist shadow 6 
        ["r.ShadowQuality"] = 5,

    },

    ["Balanced"] = {

        ["foliage.OnlyLOD"] = -1,

        --path resolution 1
        ["foliage.DensityScale"] = 0,
        --path recover article distance 2
        ["foliage.LODDistanceScale"] = 0.8,
        --low resolition +fps 3
        ["r.ScreenPercentage"] = 90,
        --up fps 4
        ["foliage.DensityScale"] = 1,
        --relist ear 5
        ["r.Shadow.DistanceScale"] = 0.80,
        --realist shadow 6 
        ["r.ShadowQuality"] = 3,

    },

    ["Performance"] = {

        ["foliage.OnlyLOD"] = -1,

        --path resolution 1
        ["foliage.DensityScale"] = 0,
        --path recover article distance 2
        ["foliage.LODDistanceScale"] = 0.7,
        --low resolition +fps 3
        ["r.ScreenPercentage"] = 80,
        --up fps 4
        ["foliage.DensityScale"] = 0.8,
        --relist ear 5
        ["r.Shadow.DistanceScale"] = 0.75,
        --realist shadow 6 
        ["r.ShadowQuality"] = 2,

    },

    ["Ultra Performance"] = {

        ["foliage.OnlyLOD"] = -1,    --   -1

        --path resolution 1
        ["foliage.DensityScale"] = 0,
        --path recover article distance 2
        ["foliage.LODDistanceScale"] = 0.6,
        --low resolition +fps 3
        ["r.ScreenPercentage"] = 70,
        --up fps 4
        ["foliage.DensityScale"] = 0.5,
        --relist ear 5
        ["r.Shadow.DistanceScale"] = 0.75,
        --realist shadow 6 
        ["r.ShadowQuality"] = 2,

    },

}

----------------------------------------------------------
-- TransitionMap
----------------------------------------------------------
MapOverrides["Level /Game/Map/TransitionMap/TransitionMap.TransitionMap:PersistentLevel"] = {

    ["Vanilla Graphics"] = {

        ["r.LightFunctionQuality"] = 2,
        ["r.EyeAdaptationQuality"] = 1,
        ["r.Lumen.HardwareRayTracing"] = 1,
        ["r.ShadowQuality"] = 3,
        ["r.Shadow.CSM.MaxCascades"] = 3,
        ["r.Shadow.DistanceScale"] = 0.16,
        ["r.Shadow.MaxCSMResolution"] = 1024,
        ["foliage.OnlyLOD"] = -1,
        ["r.VolumetricCloud"] = 1,
        ["foliage.DitheredLOD"] = 1,
        ["foliage.LODDistanceScale"] = 1,
        ["foliage.MinimumScreenSize"] = 0.000005,

    },

    ["Balanced"] = {

        ["r.LightFunctionQuality"] = 2,
        ["r.EyeAdaptationQuality"] = 1,
        ["r.Lumen.HardwareRayTracing"] = 0,
        ["r.ShadowQuality"] = 4,
        ["r.Shadow.CSM.MaxCascades"] = 1,
        ["r.Shadow.DistanceScale"] = 0.64,
        ["r.Shadow.MaxCSMResolution"] = 8192,
        ["foliage.OnlyLOD"] = 0,
        ["r.VolumetricCloud"] = 0,
        ["foliage.DitheredLOD"] = 1,
        ["foliage.LODDistanceScale"] = 2,
        ["foliage.MinimumScreenSize"] = 0.000005,

    },

    ["Performance"] = {

        ["r.LightFunctionQuality"] = 2,
        ["r.EyeAdaptationQuality"] = 1,
        ["r.Lumen.HardwareRayTracing"] = 0,
        ["r.ShadowQuality"] = 4,
        ["r.Shadow.CSM.MaxCascades"] = 1,
        ["r.Shadow.DistanceScale"] = 0.64,
        ["r.Shadow.MaxCSMResolution"] = 8192,
        ["foliage.OnlyLOD"] = 0,
        ["r.VolumetricCloud"] = 0,
        ["foliage.DitheredLOD"] = 1,
        ["foliage.LODDistanceScale"] = 2,
        ["foliage.MinimumScreenSize"] = 0.000005,

    },

    ["Ultra Performance"] = {

        ["r.LightFunctionQuality"] = 2,
        ["r.EyeAdaptationQuality"] = 1,
        ["r.Lumen.HardwareRayTracing"] = 0,
        ["r.ShadowQuality"] = 0,
        ["r.Shadow.CSM.MaxCascades"] = 0,
        ["r.Shadow.DistanceScale"] = 0,
        ["r.Shadow.MaxCSMResolution"] = 1024,
        ["foliage.OnlyLOD"] = 0,
        ["r.VolumetricCloud"] = 0,
        ["foliage.DitheredLOD"] = 1,
        ["foliage.LODDistanceScale"] = 1,
        ["foliage.MinimumScreenSize"] = 0.000005,

    },

}

----------------------------------------------------------
-- WornHouse        readme.md 1
----------------------------------------------------------
MapOverrides["Level /Game/Map/WornHouse/WornHouse.WornHouse:PersistentLevel"] = {

    ["Vanilla Graphics"] = {

        --path
        ["r.Shadow.DistanceScale"] = 1,
       
    },

    ["Balanced"] = {

        --path
        ["r.Shadow.DistanceScale"] = 0.64,    

    },

    ["Performance"] = {

        --path
        ["r.Shadow.DistanceScale"] = 0.64,      

    },

    ["Ultra Performance"] = {

        --path
        ["r.Shadow.DistanceScale"] = 0.64,

    },

} 

----------------------------------------------------------
-- Hospital        readme.md 2
----------------------------------------------------------
MapOverrides["Level /Game/Map/Hospital/Hospital.Hospital:PersistentLevel"] = {

    ["Vanilla Graphics"] = {

        --path
        ["r.Shadow.DistanceScale"] = 1,
       
    },

    ["Balanced"] = {

        --path
        ["r.Shadow.DistanceScale"] = 0.64,    

    },

    ["Performance"] = {

        --path
        ["r.Shadow.DistanceScale"] = 0.64,      

    },

    ["Ultra Performance"] = {

        --path
        ["r.Shadow.DistanceScale"] = 0.64,

    },

} 

----------------------------------------------------------
-- Rome        readme.md 3
----------------------------------------------------------
MapOverrides["Level /Game/Map/Rome/Rome.Rome:PersistentLevel"] = {

    ["Vanilla Graphics"] = {

        --path lumen 1
        ["r.ShadowQuality"] = 5,
        --light lum 2   
        ["r.LightShaftQuality"] = 1,
       
    },

    ["Balanced"] = {

        --path lumen 1
        ["r.ShadowQuality"] =4,  
        --light lum 2   
        ["r.LightShaftQuality"] = 1,  

    },

    ["Performance"] = {

        --path lumen 1
        ["r.ShadowQuality"] = 4,
        --light lum 2   
        ["r.LightShaftQuality"] = 1,

    },

    ["Ultra Performance"] = {

        --path lumen 1
        ["r.ShadowQuality"] = 4,
        --light lum 2   
        ["r.LightShaftQuality"] = 1,

    },

} 

----------------------------------------------------------
-- AirSoft        readme.md 4
----------------------------------------------------------
MapOverrides["Level /Game/Map/AirSoft/AirSoft.AirSoft:PersistentLevel"] = {

    ["Vanilla Graphics"] = {

        --path lumen 1 
        ["r.Shadow.DistanceScale"] = 1,
        --path auto lumen 2
        ["r.SSR.Quality"] = 3,
        --path Denoiser 3
        ["r.Shadow.Denoiser"] = 2,
        --path 防止木板透光 4
        ["r.Shadow.Virtual.Enable"] = 1,  
        --降低整个画质来提升帧率
        ["r.ScreenPercentage"] = 100,

    },

    ["Balanced"] = {

        --path lumen 1 
        ["r.Shadow.DistanceScale"] = 0.85,
        --path auto lumen 2
        ["r.SSR.Quality"] = 3,
        --path Denoiser 3
        ["r.Shadow.Denoiser"] = 1,
        --path 防止木板透光 4
        ["r.Shadow.Virtual.Enable"] = 1,  
        --降低整个画质来提升帧率
        ["r.ScreenPercentage"] = 90,

    },

    ["Performance"] = {

        --path lumen 1 
        ["r.Shadow.DistanceScale"] = 0.75,
        --path auto lumen 2
        ["r.SSR.Quality"] = 3,
        --path Denoiser 3
        ["r.Shadow.Denoiser"] = 1,
        --path 防止木板透光 4
        ["r.Shadow.Virtual.Enable"] = 1,
        --降低整个画质来提升帧率
        ["r.ScreenPercentage"] = 80,  

    },

    ["Ultra Performance"] = {

        --path lumen 1 
        ["r.Shadow.DistanceScale"] = 0.75,
        --path auto lumen 2
        ["r.SSR.Quality"] = 3,
        --path Denoiser 3
        ["r.Shadow.Denoiser"] = 1,
        --path 防止木板透光 4
        ["r.Shadow.Virtual.Enable"] = 1,               
        --降低整个画质来提升帧率
        ["r.ScreenPercentage"] = 70,        

    },

} 

----------------------------------------------------------
-- Nick name: Backrooms        readme.md 5
-- Low-level name/program name: TheBackrooms 
-- 文件夹是Backrooms  文件名是TheBackrooms
----------------------------------------------------------
MapOverrides["Level /Game/Map/Backrooms/TheBackrooms.TheBackrooms:PersistentLevel"] = {

    ["Vanilla Graphics"] = {

        --降低整个画质来提升帧率 1
        ["r.ScreenPercentage"] = 100,
        --真实黑暗 2
        ["r.ShadowQuality"] = 5,
        --距离长赖不穿光 3
        ["r.Shadow.DistanceScale"] = 1,
        ["r.Shadow.MaxCSMResolution"] = 1024,

    },

    ["Balanced"] = {

        --降低整个画质来提升帧率 1
        ["r.ScreenPercentage"] = 90,
        --真实黑暗 2
        ["r.ShadowQuality"] = 4,
        --距离长赖不穿光 3
        ["r.Shadow.DistanceScale"] = 1,
        ["r.Shadow.MaxCSMResolution"] = 8192,

    },

    ["Performance"] = {

        --降低整个画质来提升帧率 1
        ["r.ScreenPercentage"] = 80,  
        --真实黑暗 2
        ["r.ShadowQuality"] = 4,
        --距离长赖不穿光 3
        ["r.Shadow.DistanceScale"] = 1,
        ["r.Shadow.MaxCSMResolution"] = 8192,

    },

    ["Ultra Performance"] = {

        --降低整个画质来提升帧率 1
        ["r.ScreenPercentage"] = 70,       
        --真实黑暗 2
        ["r.ShadowQuality"] = 4, 
        --距离长赖不穿光 3
        ["r.Shadow.DistanceScale"] = 1,
        ["r.Shadow.MaxCSMResolution"] = 8192,

    },

} 

----------------------------------------------------------
-- PublicPool        readme.md 6
----------------------------------------------------------
MapOverrides["Level /Game/Map/PublicPool/PublicPool.PublicPool:PersistentLevel"] = {

    ["Vanilla Graphics"] = {

        --降低整个画质来提升帧率 1
        ["r.ScreenPercentage"] = 100,
        --真实光照 2
        ["r.ShadowQuality"] = 5,
        --防止远处发光 3
        ["r.Shadow.DistanceScale"] = 1,
        --远距离渲染 4
        ["r.Shadow.RadiusThreshold"] = 0.05,
        ["r.Shadow.MaxCSMResolution"] = 1024,
    },

    ["Balanced"] = {

        --降低整个画质来提升帧率 1
        ["r.ScreenPercentage"] = 95,
        --真实光照 2
        ["r.ShadowQuality"] = 5,
        --防止远处发光 3
        ["r.Shadow.DistanceScale"] = 0.64,
        --远距离渲染 4
        ["r.Shadow.RadiusThreshold"] = 0.05,
        ["r.Shadow.MaxCSMResolution"] = 1024,

    },

    ["Performance"] = {

        --降低整个画质来提升帧率 1
        ["r.ScreenPercentage"] = 90,  
        --真实光照 2
        ["r.ShadowQuality"] = 4,
        --防止远处发光 3
        ["r.Shadow.DistanceScale"] = 0.64,
        --远距离渲染 4
        ["r.Shadow.RadiusThreshold"] = 0.05,
        ["r.Shadow.MaxCSMResolution"] = 1024,

    },

    ["Ultra Performance"] = {

        --降低整个画质来提升帧率 1
        ["r.ScreenPercentage"] = 80,       
        --真实光照 2
        ["r.ShadowQuality"] = 4,
        --防止远处发光 3
        ["r.Shadow.DistanceScale"] = 0.64,
        --远距离渲染 4
        ["r.Shadow.RadiusThreshold"] = 0.05,
        ["r.Shadow.MaxCSMResolution"] = 1024,

    },

} 

----------------------------------------------------------
-- Auto Apply
----------------------------------------------------------
local ApplyCurrentPreset
local ApplyMapOverrides


----------------------------------------------------------
-- Apply Current Preset
--
-- Applies the graphics preset selected in Configuration.
----------------------------------------------------------
ApplyCurrentPreset = function()

    print("[Preset] ApplyCurrentPreset")
    print("[Preset] Mode = " .. tostring(Mode))

    if Mode == "Vanilla Graphics" then

        ApplyVanilla()

    elseif Mode == "Balanced" then

        ApplyBalanced()

    elseif Mode == "Performance" then

        ApplyPerformance()

    elseif Mode == "Ultra Performance" then

        ApplyUltraPerformance()

    end

    ApplyMapOverrides()

    CurrentPreset = Mode

    print("[Preset] Finished")

end


----------------------------------------------------------
-- Set Graphics Preset
----------------------------------------------------------
local function SetPreset(Preset)

    Mode = Preset

    ApplyCurrentPreset()

end


----------------------------------------------------------
-- Apply Map Overrides
--
-- Applies map-specific console commands after the preset.
----------------------------------------------------------
ApplyMapOverrides = function()

    print("[Override] Begin")

    print("[Override] CurrentMap = " .. tostring(CurrentMap))

    if not CurrentMapOverride then

        print("[Override] CurrentMapOverride = nil")

        return

    end

    local Override = CurrentMapOverride[Mode]

    if not Override then

        print("[Override] No Override For Mode: " .. tostring(Mode))

        return

    end

    for Command, Value in pairs(Override) do

        print("[Override]", Command, Value)

        ExecCmd(Command, Value)

    end

    print("[Override] End")

end


----------------------------------------------------------
-- Task Queue
--
-- Executes a task immediately if the world is ready,
-- otherwise queues it for later execution.
----------------------------------------------------------
local function SubmitTask(Task)

    local World = GetWorld()

    if World
        and World:IsValid()
        and World.PersistentLevel then

        return Task()

    end

    table.insert(TaskQueue, Task)

end


----------------------------------------------------------
-- Flush Task Queue
--
-- Executes all queued tasks once the world is ready.
----------------------------------------------------------
local function FlushTaskQueue()

    local World = GetWorld()

    if not (
        World
        and World:IsValid()
        and World.PersistentLevel
    ) then
        return
    end

    for _, Task in ipairs(TaskQueue) do
        Task()
    end

    TaskQueue = {}

end


----------------------------------------------------------
-- Auto Apply Timer
----------------------------------------------------------



----------------------------------------------------------
-- Start Auto Apply
--
-- Automatically applies the selected graphics preset
-- after the configured delay.
----------------------------------------------------------
local function StartAutoApply()

    print("[Auto] StartAutoApply")

    if not AutoApply then

        print("[Auto] Disabled")

        return

    end

    ExecuteWithDelay(AutoApplyDelay * 1000, function()

        print("[Auto] Delay Finished")

        SubmitTask(function()

            print("[Auto] SubmitTask")

            ApplyCurrentPreset()

            --auto run F2 or F3 or F4
            RunAutoHotkey()

        end)

    end)

end


----------------------------------------------------------
-- Map Detection
----------------------------------------------------------
NotifyOnNewObject("/Script/Engine.Level", function(ConstructedObject)

    local RecognizedLevel = ConstructedObject:GetFullName()

    print("--------------------------------")
    print("[Map] Notify")
    print("[Map] " .. RecognizedLevel)

    LastLevelLoad = os.clock()

    local Override = MapOverrides[RecognizedLevel]

    if Override then

        print("[Map] Override Found")

        CurrentMap = RecognizedLevel
        CurrentMapOverride = Override

        if not IsInitialized then

            print("[Map] First Init")

            IsInitialized = true

            StartAutoApply()

        else

            print("[Map] ApplyCurrentPreset")

            ApplyCurrentPreset()

        end

    else

        print("[Map] No Override (Ignored)")

    end

end)


----------------------------------------------------------
-- Auto Hotkey
----------------------------------------------------------
local function RunAutoHotkey()

    if AutoHotkeyMode == "None" then
        return
    end

    local function Trigger()

        if AutoHotkeyMode == "Balanced" then

            SetPreset("Balanced")

        elseif AutoHotkeyMode == "Performance" then

            SetPreset("Performance")

        elseif AutoHotkeyMode == "Ultra Performance" then

            SetPreset("Ultra Performance")

        end

    end

    ExecuteWithDelay(AutoHotkeyStartupDelay, function()

        Trigger()

        ExecuteWithDelay(AutoHotkeyPressDelay, function()

            Trigger()

        end)

    end)

end


----------------------------------------------------------
-- Face Mosaic
----------------------------------------------------------
local FaceMosaicEnabled = false
local FaceMosaicState = true     -- 当前状态：true=有马赛克，false=无马赛克

local BLUR_MATERIAL = "MaterialInstanceConstant /Game/Mosaic_Pixelization_Material_Pack/Mosaic_Pixelization_Materials/MIPP_BodycamMosaicEffect.MIPP_BodycamMosaicEffect"

local function IsValid(Obj)
    if not Obj then return false end
    local ok, result = pcall(function() return Obj:IsValid() end)
    return ok and result
end

local function FindInstances(Type)
    if type(FindAllOf) ~= "function" then
        return {}
    end

    local ok, Instances = pcall(function()
        return FindAllOf(Type)
    end)

    if ok and Instances ~= nil then
        return Instances
    end

    return {}
end

local function ProcessBlendables(Settings, TargetWeight)
    if not Settings then return end
    
    local Blendables
    pcall(function() Blendables = Settings.WeightedBlendables.Array end)
    if not Blendables then return end
    
    local Count = 0
    pcall(function() Count = Blendables:GetArrayNum() end)
    
    for i = 1, Count do
        local Item = Blendables[i]
        if Item and IsValid(Item.Object) then
            local ok, path = pcall(function() return Item.Object:GetFullName() end)
            
            if ok and path == BLUR_MATERIAL then
                if math.abs(Item.Weight - TargetWeight) > 0.001 then
                    pcall(function()
                        Blendables[i] = { Weight = TargetWeight, Object = Item.Object }
                    end)
                end
            end
        end
    end
end

----------------------------------------------------------
-- Update Face Mosaic
----------------------------------------------------------
local function UpdateFaceMosaic()

    local Weight = FaceMosaicState and 1.0 or 0.0

    for _, Type in ipairs({
        "PostProcessVolume",
        "PostProcessComponent",
        "CameraComponent"
    }) do

        local Instances = FindInstances(Type)

        for _, Obj in ipairs(Instances) do

            if IsValid(Obj) then

                ProcessBlendables(Obj.Settings, Weight)
                ProcessBlendables(Obj.PostProcessSettings, Weight)

            end

        end

    end

    print("[Mosaic] Weight = " .. tostring(Weight))

end

----------------------------------------------------------
-- Toggle Face Mosaic
----------------------------------------------------------
local function ToggleFaceMosaic()

    -- 工具箱关闭了此功能
    if not FaceMosaicEnabled then
        return
    end

    FaceMosaicState = not FaceMosaicState

    print("[Mosaic] Toggle")

    UpdateFaceMosaic()

    print("[Mosaic] " .. (FaceMosaicState and "Enabled" or "Disabled"))

end



----------------------------------------------------------
-- Full motion blur is prohibited.全动态模糊禁止
----------------------------------------------------------
local FullDynamicBlurEnabled = false -- 工具箱控制是否启用F10功能
local FullDynamicBlur = false -- 当前状态（false=默认开启模糊）

local function ToggleFullDynamicBlur()

    -- 功能被关闭，直接返回
    if not FullDynamicBlurEnabled then
        return
    end

    FullDynamicBlur = not FullDynamicBlur

    ExecCmd(
        "r.PostProcessing.DisableMaterials",
        FullDynamicBlur and 0 or 1
    )

    print("[F10] Full Dynamic Blur " ..
        (FullDynamicBlur and "Disabled" or "Enabled"))

end


----------------------------------------------------------
-- Keybinds
----------------------------------------------------------
RegisterKeyBind(Key.F1, function()
    SetPreset("Vanilla Graphics")
end)

RegisterKeyBind(Key.F2, function()
    SetPreset("Balanced")
end)

RegisterKeyBind(Key.F3, function()
    SetPreset("Performance")
end)

RegisterKeyBind(Key.F4, function()
    SetPreset("Ultra Performance")
end)    

RegisterKeyBind(Key.F9, ToggleFaceMosaic)

RegisterKeyBind(Key.F10, ToggleFullDynamicBlur)