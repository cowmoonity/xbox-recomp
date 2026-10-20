namespace SuperCow;

public class CRand
{
	public static byte[] rand_array_ = new byte[256]
	{
		16, 28, 172, 31, 52, 188, 90, 233, 27, 19,
		232, 89, 56, 157, 170, 155, 130, 208, 45, 61,
		228, 132, 115, 249, 215, 51, 102, 123, 152, 26,
		150, 143, 148, 179, 4, 223, 69, 224, 93, 5,
		2, 63, 142, 169, 88, 85, 13, 190, 244, 24,
		230, 126, 222, 66, 47, 144, 37, 213, 73, 147,
		241, 240, 86, 183, 10, 159, 239, 82, 17, 203,
		250, 55, 211, 74, 151, 77, 99, 236, 182, 231,
		140, 112, 146, 225, 116, 212, 18, 43, 251, 122,
		50, 100, 202, 60, 107, 137, 221, 192, 171, 156,
		44, 238, 174, 205, 168, 235, 124, 194, 23, 70,
		42, 95, 176, 153, 84, 189, 245, 200, 30, 20,
		254, 136, 247, 219, 14, 29, 175, 41, 32, 255,
		226, 65, 135, 22, 67, 72, 243, 252, 80, 0,
		166, 181, 160, 33, 185, 161, 204, 46, 76, 81,
		248, 209, 128, 184, 229, 91, 237, 75, 83, 149,
		186, 141, 199, 253, 9, 68, 117, 64, 106, 3,
		39, 6, 198, 134, 242, 214, 218, 227, 21, 49,
		36, 217, 92, 97, 180, 138, 164, 48, 154, 119,
		38, 109, 120, 127, 111, 165, 7, 87, 1, 94,
		11, 196, 145, 114, 58, 195, 96, 121, 110, 118,
		12, 105, 113, 167, 101, 173, 210, 53, 187, 193,
		34, 163, 8, 246, 57, 158, 220, 131, 191, 62,
		234, 139, 54, 162, 108, 178, 98, 197, 78, 133,
		129, 177, 125, 201, 206, 40, 35, 104, 15, 71,
		59, 79, 207, 103, 25, 216
	};

	public static int lo_rand_ = 0;

	public static int hi_rand_ = 0;

	public static int hi_rand2 = 0;

	public static int lo_rand2 = 0;

	public static int phys_rand_ = 0;

	public static int[][] randposstack_ = new int[2][]
	{
		new int[256],
		new int[256]
	};

	public static int randposstack_pos_ = 0;

	public static void pushrandpos()
	{
		randposstack_[0][randposstack_pos_] = lo_rand_;
		randposstack_[1][randposstack_pos_] = hi_rand_;
		randposstack_pos_++;
		if (randposstack_pos_ > 255)
		{
			randposstack_pos_ = 255;
		}
	}

	public static void poprandpos()
	{
		randposstack_pos_--;
		if (randposstack_pos_ < 0)
		{
			randposstack_pos_ = 0;
		}
		lo_rand_ = randposstack_[0][randposstack_pos_];
		hi_rand_ = randposstack_[1][randposstack_pos_];
	}

	public static void randomize(int position)
	{
		hi_rand_ = rand_array_[position % 256];
		lo_rand_ = rand_array_[position / 256];
	}

	public static void frandomize(int position)
	{
		hi_rand_ = rand_array_[position % 256];
		lo_rand_ = rand_array_[position / 256];
	}

	public static void randomize2(int position)
	{
		hi_rand2 = rand_array_[position % 256];
		lo_rand2 = rand_array_[position / 256];
	}

	public static int getrandpos()
	{
		return hi_rand_ * 256 + lo_rand_;
	}

	public static int irand(int n)
	{
		hi_rand_ = (hi_rand_ + 1) % 256;
		lo_rand_ = (lo_rand_ + 3) % 256;
		return (256 * rand_array_[hi_rand_] + rand_array_[lo_rand_]) % n;
	}

	public static int irand2(int n)
	{
		hi_rand2 = (hi_rand2 + 1) % 256;
		lo_rand2 = (lo_rand2 + 3) % 256;
		return (256 * rand_array_[hi_rand2] + rand_array_[lo_rand2]) % n;
	}

	public static float frand(float n)
	{
		hi_rand_ = (hi_rand_ + 1) % 256;
		lo_rand_ = (lo_rand_ + 3) % 256;
		float num = 256 * rand_array_[hi_rand_] + rand_array_[lo_rand_];
		return num / 32768f * n - n;
	}

	public static float frand2(float n)
	{
		hi_rand2 = (hi_rand2 + 1) % 256;
		lo_rand2 = (lo_rand2 + 3) % 256;
		float num = 256 * rand_array_[hi_rand2] + rand_array_[lo_rand2];
		return num / 32768f * n - n;
	}

	public static float prand(float n)
	{
		hi_rand_ = (hi_rand_ + 1) % 256;
		lo_rand_ = (lo_rand_ + 3) % 256;
		float num = 256 * rand_array_[hi_rand_] + rand_array_[lo_rand_];
		return num * n / 65536f;
	}

	public static float prand2(float n)
	{
		hi_rand2 = (hi_rand2 + 1) % 256;
		lo_rand2 = (lo_rand2 + 3) % 256;
		float num = 256 * rand_array_[hi_rand2] + rand_array_[lo_rand2];
		return num * n / 65536f;
	}

	public static void setphysrandpos(int pos)
	{
		phys_rand_ = pos;
	}

	public static int phys_rand(int n)
	{
		phys_rand_ = (phys_rand_ + 23) % 256;
		return rand_array_[phys_rand_] % n;
	}
}
