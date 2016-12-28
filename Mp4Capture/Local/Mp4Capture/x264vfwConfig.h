
#define X264_MAX(a, b) (((a)>(b)) ? (a) : (b))
#define X264_CLIP(v, min, max) (((v)<(min)) ? (min) : ((v)>(max)) ? (max) : (v))
#define MAX_PATH          260
#define MAX_STATS_PATH   (MAX_PATH - 5) /* -5 because x264 add ".temp" for temp file */
#define MAX_OUTPUT_PATH  MAX_PATH
#define MAX_STATS_SIZE   X264_MAX(MAX_STATS_PATH, MAX_PATH)
#define MAX_OUTPUT_SIZE  X264_MAX(MAX_OUTPUT_PATH, MAX_PATH)
#define MAX_CMDLINE      4096

#define X264VFW_REG_KEY    HKEY_CURRENT_USER
#define X264VFW_REG_PARENT "Software\\GNU"
#ifdef _WIN64
#define X264VFW_REG_CHILD  "x264vfw64"
#else
/* Not "x264vfw" because of GordianKnot compatibility */
#define X264VFW_REG_CHILD  "x264"
#endif
#define X264VFW_REG_CLASS  "config"

/* CONFIG: VFW config */
typedef struct
{
    int i_format_version;
    /* Basic */
    int i_preset;
    int i_tuning;
    int i_profile;
    int i_level;
    int b_fastdecode;
    int b_zerolatency;
    /* Rate control */
    int i_encoding_type;
    int i_qp;
    int i_rf_constant;  /* 1pass VBR, nominal QP */
    int i_passbitrate;
    int i_pass;
    int b_fast1pass;    /* Turns off some flags during 1st pass */
    int b_createstats;  /* Creates the statsfile in single pass mode */
    int b_updatestats;  /* Updates the statsfile during 2nd pass */
    char stats[MAX_STATS_SIZE];
    /* Output */
    int i_output_mode;
    int i_fourcc;
#if X264VFW_USE_VIRTUALDUB_HACK
    int b_vd_hack;
#else
    int reserved_b_vd_hack;
#endif
    char output_file[MAX_OUTPUT_SIZE];
    /* Sample Aspect Ratio */
    int i_sar_width;
    int i_sar_height;
    /* Debug */
    int i_log_level;
    int b_psnr;
    int b_ssim;
    int b_no_asm;
    /* Decoder && AVI Muxer */
#if defined(HAVE_FFMPEG) && X264VFW_USE_DECODER
    int b_disable_decoder;
#else
    int reserved_b_disable_decoder;
#endif
    /* Extra command line */
    char extra_cmdline[MAX_CMDLINE];
} CONFIG;

/* Limits */
#define MAX_QUANT   51
#define MAX_BITRATE 999999

#define COUNT_PRESET  10
#define COUNT_TUNE    7
#define COUNT_PROFILE 4
#define COUNT_LEVEL   17
#define COUNT_FOURCC  7

/* Types */
typedef struct
{
    const char * const name;
    const DWORD value;
} named_fourcc_t;

typedef struct
{
    const char * const name;
    const int value;
} named_int_t;


/* Types */
typedef struct
{
    const char * const name;
    const char * const value;
} named_str_t;

/* Registery handling */
typedef struct
{
    const char * const reg_value;
    int * const config_int;
    const char * const default_str;
    const named_str_t * const list;
    const int list_count;
} reg_named_str_t;

typedef struct
{
    const char * const reg_value;
    int * const config_int;
    const int default_int;
    const int min_int;
    const int max_int;
} reg_int_t;

typedef struct
{
    const char * const reg_value;
    char * const config_str;
    const char * const default_str;
    const int max_len; /* Maximum string length, including the terminating NULL char */
} reg_str_t;

static CONFIG reg;

const named_str_t x264vfw_preset_table[COUNT_PRESET] =
{
    { "Ultrafast", "ultrafast" },
    { "Superfast", "superfast" },
    { "Veryfast",  "veryfast"  },
    { "Faster",    "faster"    },
    { "Fast",      "fast"      },
    { "Medium",    "medium"    },
    { "Slow",      "slow"      },
    { "Slower",    "slower"    },
    { "Veryslow",  "veryslow"  },
    { "Placebo",   "placebo"   }
};

const named_str_t x264vfw_tune_table[COUNT_TUNE] =
{
    { "None",        ""           },
    { "Film",        "film"       },
    { "Animation",   "animation"  },
    { "Grain",       "grain"      },
    { "Still image", "stillimage" },
    { "PSNR",        "psnr"       },
    { "SSIM",        "ssim"       }
};

const named_str_t x264vfw_profile_table[COUNT_PROFILE] =
{
    { "Auto",     ""         },
    { "Baseline", "baseline" },
    { "Main",     "main"     },
    { "High",     "high"     }
};




//static const reg_named_str_t reg_named_str_table[] =
//{
//    
//
//    { "preset",  &reg.i_preset,  "medium", x264vfw_preset_table,  COUNT_PRESET  },
//    { "tuning",  &reg.i_tuning,  "",       x264vfw_tune_table,    COUNT_TUNE    },
//    { "profile", &reg.i_profile, "",       x264vfw_profile_table, COUNT_PROFILE }
//};

static const reg_int_t reg_int_table[] =
{
    /* Basic */
    /*{ "avc_level",       &reg.i_level,           0,   0,  COUNT_LEVEL - 1  },
    { "fastdecode",      &reg.b_fastdecode,      0,   0,  1                },*/
    { "zerolatency",     &reg.b_zerolatency,     0,   0,  1                },
    /* Rate control */
    { "encoding_type",   &reg.i_encoding_type,   4,   0,  4                }, /* Take into account GordianKnot workaround */
   /* { "quantizer",       &reg.i_qp,              23,  1,  MAX_QUANT        },
    { "ratefactor",      &reg.i_rf_constant,     230, 10, MAX_QUANT * 10   },*/
    { "passbitrate",     &reg.i_passbitrate,     800, 1,  MAX_BITRATE      },
    { "pass_number",     &reg.i_pass,            1,   1,  2                },
    //{ "fast1pass",       &reg.b_fast1pass,       0,   0,  1                },
    //{ "createstats",     &reg.b_createstats,     0,   0,  1                },
    //{ "updatestats",     &reg.b_updatestats,     1,   0,  1                },
    ///* Output */
    //{ "output_mode",     &reg.i_output_mode,     0,   0,  1                },
    //{ "fourcc_num",      &reg.i_fourcc,          0,   0,  COUNT_FOURCC - 1 },
//#if X264VFW_USE_VIRTUALDUB_HACK
//    { "vd_hack",         &reg.b_vd_hack,         0,   0,  1                },
//#endif
//    /* Sample Aspect Ratio */
    { "sar_width",       &reg.i_sar_width,       1,   1,  9999             },
    { "sar_height",      &reg.i_sar_height,      1,   1,  9999             }
//    /* Debug */
//    { "log_level",       &reg.i_log_level,       2,   0,  4                },
//    { "psnr",            &reg.b_psnr,            1,   0,  1                },
//    { "ssim",            &reg.b_ssim,            1,   0,  1                },
//    { "no_asm",          &reg.b_no_asm,          0,   0,  1                },
//    /* Decoder && AVI Muxer */
//#if defined(HAVE_FFMPEG) && X264VFW_USE_DECODER
//    { "disable_decoder", &reg.b_disable_decoder, 0,   0,  1                }
//#endif
};

//static const reg_str_t reg_str_table[] =
//{
//    /* Rate control */
//    { "statsfile",     reg.stats,         ".\\x264.stats", MAX_STATS_PATH  },
//    /* Output */
//    { "output_file",   reg.output_file,   "",              MAX_OUTPUT_PATH },
//    /* Extra command line */
//    { "extra_cmdline", reg.extra_cmdline, "",              MAX_CMDLINE     }
//};

#define GordianKnotWorkaround(encoding_type)\
do {\
    switch (encoding_type)\
    {\
        case 0:\
          encoding_type = 3;\
          break;\
        case 1:\
          encoding_type = 1;\
          break;\
        case 2:\
          encoding_type = 4;\
          break;\
        case 3:\
          encoding_type = 0;\
          break;\
        case 4:\
          encoding_type = 2;\
          break;\
        default:\
          assert(0);\
          break;\
    }\
} while (0)