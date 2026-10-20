using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SuperCow;

public static class SCGlobals
{
	public static int MAX_MONSTER_OF_ONE_TYPE = 64;

	public static CPersSnail[] ar_CPersSnail = new CPersSnail[MAX_MONSTER_OF_ONE_TYPE];

	public static CPersMole[] ar_CPersMole = new CPersMole[MAX_MONSTER_OF_ONE_TYPE];

	public static CPersDog[] ar_CPersDog = new CPersDog[MAX_MONSTER_OF_ONE_TYPE];

	public static CPersBird[] ar_CPersBird = new CPersBird[MAX_MONSTER_OF_ONE_TYPE];

	public static CPersBird2[] ar_CPersBird2 = new CPersBird2[MAX_MONSTER_OF_ONE_TYPE];

	public static CPersSpider[] ar_CPersSpider = new CPersSpider[MAX_MONSTER_OF_ONE_TYPE];

	public static CPersSpider[] ar_CPersSpider2 = new CPersSpider[MAX_MONSTER_OF_ONE_TYPE];

	public static CPersHedgehog[] ar_CPersHedgehog = new CPersHedgehog[MAX_MONSTER_OF_ONE_TYPE];

	public static CPersFrog[] ar_CPersFrog = new CPersFrog[MAX_MONSTER_OF_ONE_TYPE];

	public static CPersBat[] ar_CPersBat = new CPersBat[MAX_MONSTER_OF_ONE_TYPE];

	public static CPersWasp[] ar_CPersWasp = new CPersWasp[MAX_MONSTER_OF_ONE_TYPE];

	public static CPersBug[] ar_CPersBug = new CPersBug[MAX_MONSTER_OF_ONE_TYPE];

	public static CPersFlyCatcher[] ar_CPersFlyCatcher = new CPersFlyCatcher[MAX_MONSTER_OF_ONE_TYPE];

	public static CPersPig[] ar_CPersPig = new CPersPig[MAX_MONSTER_OF_ONE_TYPE - 61];

	public static CPersGoat[] ar_CPersGoat = new CPersGoat[MAX_MONSTER_OF_ONE_TYPE - 61];

	public static CPersProf[] ar_CPersProf = new CPersProf[MAX_MONSTER_OF_ONE_TYPE - 61];

	public static Matrix mGlobalTransform = Matrix.CreateTranslation(160f, 0f, 0f) * Matrix.CreateScale(1.2f, 1.2f, 0f);

	public static float mProgress = 0f;

	public static int menuSpeed = 400;

	public static SpriteFont FontVerdana14 = null;

	public static SpriteFont FontVerdana20 = null;

	public static SpriteFont FontComicSans14 = null;

	public static CMonster[] ar_monsters;

	public static int nummonsters;

	public static int[] ar_nummonsterstype = new int[16];

	public static int numobstacles;

	public static obstacle_t[] ar_obstacles = new obstacle_t[2048];

	public static bool b_upsellActive;

	public static CMainHero hero = new CMainHero();

	public static CMainHero heroMenu = new CMainHero();

	public static string[] ar_herowords = new string[5];

	public static uint control_state;

	public static material_t matcommon = default(material_t);

	public static material_t gMaterial = default(material_t);

	public static light_t gLight = new light_t();

	public static bool gLightEnable = false;

	public static bool gLighting = false;

	public static bool bTextureExists = false;

	public static TextureList gtexlist = new TextureList();

	public static CSkeletonHost skeletonhost = new CSkeletonHost();

	public static CTextureHost texturehost = new CTextureHost();

	public static CGeomObjHost geomobjhost = new CGeomObjHost();

	public static CPersHost pershost = new CPersHost();

	public static CAnimHost animhost = new CAnimHost();

	public static CMeshHost meshhost = new CMeshHost();

	public static int currentLayer = 5;

	public static AnimateBack animbacks = new AnimateBack();

	public static SuperCowGame supercowgame;

	public static gameobjgroup_t[] group_names = new gameobjgroup_t[128];

	public static int numobjgroups = 0;

	public static gameobject_t[] ar_gameobjs = new gameobject_t[512];

	public static float global_timer;

	public static int tip_read_timer = 0;

	public static bool b_stopphysics = false;

	public static int iglobal_time = 0;

	public static color_t global_2d_color = new color_t(1f, 1f, 1f, 1f);

	public static bool b_shadows = false;

	public static string gDirectory;

	public static matrix_t gProjectionMatrix = default(matrix_t);

	public static matrix_t gViewMatrix = default(matrix_t);

	public static matrix_t gWorldMatrix = default(matrix_t);

	public static matrix_t projection = default(matrix_t);

	public static matrix_t cam = default(matrix_t);

	public static levelstat_t levelstat = default(levelstat_t);

	public static bool b_trophy4was_not_punished = true;

	public static CGeomObj p_banjocgo;

	public static CFrustum frustum = new CFrustum();

	public static vec2_t camerapos = default(vec2_t);

	public static vec2_t cameraposModifier = default(vec2_t);

	public static vec2_t prevcamerapos;

	public static vec2_t selection0;

	public static vec2_t selection1;

	public static float GRID_STEP_X = 40f;

	public static float GRID_STEP_Y = 40f;

	public static float GRID_SIZE_X = 800f;

	public static float GRID_SIZE_Y = 600f;

	public static float GRID_SIZE_X0 = 800f;

	public static float GRID_SIZE_Y0 = 600f;

	public static bool b_snaptogrid = false;

	public static float le_plane_scale = 1f;

	public static int draw_plane_index = 1;

	public static int le_state = 0;

	public static dcolor_t color_selected = default(dcolor_t);

	public static frect_t viewport_rect = default(frect_t);

	public static texture_t p_white;

	public static texture_t p_happyendback;

	public static texture_t p_happyend0;

	public static texture_t p_happyend1;

	public static texture_t p_menuballoon;

	public static texture_t p_menutex;

	public static texture_t p_mainshadow;

	public static texture_t p_a;

	public static texture_t p_b;

	public static texture_t p_x;

	public static texture_t p_start;

	public static CLevelObj p_addedobj;

	public static bool b_dynamicobj = false;

	public static bool b_gamemenu = false;

	public static bool b_pause = false;

	public static material_t matselected = default(material_t);

	public static int scoreatlevel = 0;

	public static bool b_gamepause = false;

	public static bool b_completegame = false;

	public static float trophytimer = 0f;

	public static int numlives = 3;

	public static int numgameobjs;

	public static bool b_le_active = false;

	public static CGameLevel level = new CGameLevel();

	public static int curleveltask = 0;

	public static int curlevelmusic = 0;

	public static int levelcompleteness = 0;

	public static levelback_t[] ar_levelbacks = new levelback_t[32];

	public static int numlevelbacks;

	public static int curlevelback;

	public static texture_t p_podkladka;

	public static float menusetk = 0f;

	public static int menustatnum = 0;

	public static float menustattimer = 0f;

	public static dcolor_t col_selected = new dcolor_t(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

	public static CSprite[] menugem = new CSprite[3];

	public static texture_t p_magglass;

	public static float firet = 0f;

	public static float menuscenetime = 0f;

	public static float menubacktime = 0f;

	public static texture_t[] ar_flowers = new texture_t[10];

	public static texture_t[] ar_fruittexs = new texture_t[3];

	public static texture_t[] ar_leaves = new texture_t[3];

	public static texture_t p_toilettop;

	public static texture_t p_electric;

	public static texture_t[] p_trampo = new texture_t[10];

	public static texture_t p_waterup;

	public static texture_t p_waterfall2;

	public static texture_t p_bridge1;

	public static texture_t[] p_guillo_up = new texture_t[2];

	public static texture_t[] p_guillo_down = new texture_t[2];

	public static texture_t p_liana_tex_up;

	public static texture_t p_liana_tex_down;

	public static texture_t p_lighttex;

	public static texture_t[,] p_checkpoint = new texture_t[3, 3];

	public static texture_t p_heroarrow;

	public static texture_t flaretexture;

	public static texture_t ground_textures;

	public static int NUM_WATER_TEX = 22;

	public static texture_t[] ar_watertex = new texture_t[22];

	public static int ENGINE_FREQ = 33;

	public static texture_t p_particlestex;

	public static int numVisParticles = 0;

	public static int numBlendingChanges = 0;

	public static float setpausetimer = 1f;

	public static float ftickperframe = 0f;

	public static float ftickperframe2 = 0f;

	public static uint tickperframe = 0u;

	public static uint engine_frequency;

	public static float physics_speed;

	public static float physics_speed2;

	public static float engine_speed = 1f;

	public static float frameinterp = 0f;

	public static int TimeRemainder = 0;

	public static uint lasttime = (uint)Environment.TickCount;

	public static uint lasttick = 0u;

	public static int PhysicsTime = 0;

	public static int CountRenderStateChanges = 0;

	public static bool gLevelLoading = false;

	public static vec2_t curspos = new vec2_t(0f, 0f);

	public static bool b_cheatzone = false;

	public static bool b_victory = false;

	public static light_t light = new light_t();

	public static bool b_needsetlight = false;

	public static frect_t tc_normal = new frect_t(0f, 0f, 1f, 1f);

	public static frect_t tc_inverse = new frect_t(1f, 0f, 0f, 1f);

	public static color_t color_white = new color_t(1f, 1f, 1f, 1f);

	public static Vector4 vv = default(Vector4);

	public static frect_t[] ar_wintexcoords = new frect_t[6];

	public static vec2_t[,] mitexcoords = new vec2_t[24, 4];

	public static vec2_t[] misizes = new vec2_t[24];

	public static gamecredits_t[] gamecredits = new gamecredits_t[32];

	public static bool b_exitGame = false;

	public static bool bFromMenu = false;

	public static bool bMUError = false;

	public static bool bMUErrorVisible = false;

	public static bool bGameLoaded = false;

	public static bool bInTrophyRoom = false;
}
