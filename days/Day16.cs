namespace aoc.Days;
class Day16
{
    public string Data(string path)
    {
        var data = Lib.LoadFile(path).First();
        var binarystring = String.Join(String.Empty,
          data.Select(
                  c => Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0')
                  )
        );
        return binarystring;
    }

    public long Part1(string binstring)
    {

        //971
        var (left, packets) = ProcessString(binstring, 0);
        var ii = packets.SelectMany(p => p.DeepHierachy().Select(v => v.Version)).Sum();
        return ii;
    }

    public long Part2(string binstring)
    {
        var (left, packets) = ProcessString(binstring, 0);
        var ii = packets.Select(p => p.Calculate()).Sum();
        return ii;
    }

    public (int, List<Packet>) ProcessString(string binstring, int maxPackets)
    {
        List<Packet> packets = new List<Packet>();
        var b_queue = new Queue<char>(binstring.ToCharArray());

        try
        {

            while (b_queue.Any())
            {
                if (maxPackets != 0 && maxPackets == packets.Count)
                    break;

                var packet_version = DeQueueChunk(b_queue, 3).BinToLong();
                var type_id = DeQueueChunk(b_queue, 3).BinToLong();
                Packet p = new Packet(packet_version, type_id);
                packets.Add(p);



                if (p.Type == 4)
                {
                    var i = ProcessLiteral(b_queue);
                    p.Literals.Add(i);
                }
                else
                {

                    //This is an operator packet. 
                    /*            
                    If the length type ID is 0, 
                        then the next 15 bits are a number that represents the total 
                        length in bits of the sub-packets contained by this packet.
                    If the length type ID is 1, 
                        then the next 11 bits are a number that represents the number 
                        of sub-packets immediately contained by this packet.
                    */
                    if (DeQueueChunk(b_queue, 1) == "0")
                    {
                        var sub_packets_bit_len = DeQueueChunk(b_queue, 15).BinToLong();
                        var sub_packet_string = DeQueueChunk(b_queue, sub_packets_bit_len);
                        var (processed, sub_packets) = ProcessString(sub_packet_string, 0);
                        p.SubPackets.AddRange(sub_packets);
                    }
                    else
                    {
                        //We have a number of literals
                        var collector = new List<long>();
                        var sub_packets_len = DeQueueChunk(b_queue, 11).BinToLong();
                        // I have to find the series of packet how do I find the bounds of each packet
                        // Do I just proces the string until I have X packets 
                        string the_rest = DeQueueToEnd(b_queue);
                        var (processed, sub_packets) = ProcessString(the_rest, (int)sub_packets_len);
                        p.SubPackets.AddRange(sub_packets);

                        b_queue = new Queue<char>(the_rest.Substring(processed));
                    }
                }
            }
        }
        catch (System.InvalidOperationException e)
        {
            //We just swallow the end of chucking to deal with the hex challenges. 
            //Console.WriteLine("ERROR {0}", e.Message);
        }

        return (binstring.Length - b_queue.Count, packets);
    }


    long ProcessLiteral(Queue<char> b_queue)
    {
        var lit = String.Empty;
        var keepGoing = true;
        while (keepGoing)
        {
            keepGoing = DeQueueChunk(b_queue, 1) == "1";
            lit += DeQueueChunk(b_queue, 4);
        }
        return lit.BinToLong();
    }

    string DeQueueChunk(Queue<char> q_string, long chunkSize)
    {
        var s = String.Empty;
        for (int i = 0; i < chunkSize; i++)
        {
            s += q_string.Dequeue().ToString();
        }
        return s;
    }
    string DeQueueToEnd(Queue<char> q_string)
    {
        var s = String.Empty;
        while (q_string.Any())
            s += q_string.Dequeue().ToString();
        return s;
    }
}



public static class HelperExtensions
{
    public static long BinToLong(this String s)
    {
        return Convert.ToInt64(s, 2);
    }

}


public class Packet
{
    public Packet(long version, long type)
    {
        this.Type = type;
        this.Version = version;
        this.Literals = new List<long>();
        this.SubPackets = new List<Packet>();
    }
    public long Type { get; set; }
    public long Version { get; set; }

    public List<long> Literals { get; set; }

    public List<Packet> SubPackets { get; set; }

    public long Calculate()
    {
        switch (this.Type)
        {
            //Packets with type ID 0 are sum packets - their value is the sum of the values of their sub-packets. 
            //If they only have a single sub-packet, their value is the value of the sub-packet.

            case 0: //Sum Packets

                return this.SubPackets.Sum(p => p.Calculate());
            case 1:
                //Packets with type ID 1 are product packets - their value is the result of multiplying together 
                //the values of their sub-packets.
                // If they only have a single sub-packet, their value is the value of the sub-packet.
                return this.SubPackets.Aggregate(1L, (v, s) => v * s.Calculate());
            case 2:
                //Packets with type ID 2 are minimum packets
                return this.SubPackets.Min(p => p.Calculate());
            case 3:
                //Packets with type ID 3 are maximum packet
                return this.SubPackets.Max(p => p.Calculate());
            case 4: //Product
                return this.Literals.Sum();
            case 5:
                ////Packets with type ID 5 
                //are greater than packets - their value is 1 if the value of the first sub-packet is greater 
                //than the value of the second sub-packet; otherwise, their value is 0. These packets always have exactly two sub-packets. 
                return this.SubPackets.First().Calculate() > this.SubPackets.Last().Calculate() ? 1 : 0;
            case 6:
                //Packets with type ID 6 are less than packets - their value is 1 if the value of the first sub-packet is less than the value of the second sub-packet; 
                //otherwise, their value is 0. These packets always have exactly two sub-packets.
                return this.SubPackets.First().Calculate() < this.SubPackets.Last().Calculate() ? 1 : 0;
            case 7: //Product
                return this.SubPackets.First().Calculate() == this.SubPackets.Last().Calculate() ? 1 : 0;
            default:
                throw new ArgumentException(this.Type + " -- Type Mapping Does not Exist");
        }
    }

    public IEnumerable<Packet> DeepHierachy()
    {
        yield return this;

        foreach (var c in this.SubPackets)
        {
            foreach (var d in c.DeepHierachy())
            {
                yield return d;
            }
        }
    }
}



