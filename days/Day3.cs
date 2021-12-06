using System.Collections;
namespace aoc.Days;

class Day3
{
    public object Data(string file)
    {
        var d = Lib.LoadList<BitArray>(file,  Convert);
        return d;
    }
    public Func<string?, BitArray> Convert = delegate (string? s)
    {
        var parts = s.ToCharArray().Select(p => p == '1').ToArray();
        return new BitArray(parts);
    };


    public int Part1(List<BitArray> data)
    {
        //Whats the length of the BitArrays
        var gamma_array = new BitArray(data[0].Length);
        var epsilon_array = new BitArray(data[0].Length);

        for (int i = 0; i < data[0].Length; i++)
        {
            var position = msb_at_positon(data, i);
            gamma_array.Set(i, position);
            epsilon_array.Set(i, !position);
        }

        var gamma_rate = bits_to_int(gamma_array);
        var epsilon_rate = bits_to_int(epsilon_array);

        return gamma_rate * epsilon_rate;
    }
    public int Part2(List<BitArray> data)
    {
        var ogr_ba = find_one(data, true);
        var oxygen_generator_rating = bits_to_int(ogr_ba);

        var c02_scrubber_ba = find_one(data, false);
        var co2_scrubber_rating = bits_to_int(c02_scrubber_ba);

        return co2_scrubber_rating * oxygen_generator_rating;
    }

    private string bits_to_string(BitArray ba)
    {

        var s = "";
        foreach (bool b in ba)
        {
            s += b ? "1" : "0";
        }
        return s;
    }
    private int bits_to_int(BitArray ba)
    {
        var s = bits_to_string(ba);
        return System.Convert.ToInt32(s, 2);
    }

    private BitArray find_one(List<BitArray> data, bool msb)
    {
        var dcurrent = data.ToList();
        var current_index = 0;

        while (true)
        {
            var sig_bit = msb ? msb_at_positon(dcurrent, current_index) : lsb_at_positon(dcurrent, current_index);

            dcurrent = dcurrent.Where(p => p[current_index] == sig_bit).ToList();
            current_index += 1;


            if (dcurrent.Count() == 1)
            {
                return dcurrent.First();
            }
        }
    }

    static bool msb_at_positon(List<BitArray> ba, int postion)
    {
        return ba.Where(p => p[postion]).Count() >= ba.Where(p => !p[postion]).Count();
    }
    static bool lsb_at_positon(List<BitArray> ba, int postion)
    {
        return ba.Where(p => p[postion]).Count() < ba.Where(p => !p[postion]).Count();
    }

}
