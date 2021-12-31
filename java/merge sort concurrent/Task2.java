import java.util.Arrays;

/* Task2: no slicing, no bullshit memcopy, parralelized merge */
public class Task2 {

  /* Create new sorted array by merging 2 smaller sorted arrays */
  private static void merge(int[] src, int[] dst, int idx1, int idx2, int end) {
		// TODO: 'src' is sorted between [idx1,idx2) and [idx2,end)
    //       copy both to 'dst' in a way that [idx1,end) is sorted for 'dst'
    // Note: 'idx1' is the starting point of the 1st array
    //       'idx2' is the starting point of the 2nd array
    //       'end' is the end of the 2nd array (exclusive)
    //       There are no elements between the first and second arrays
    //       'src' is the source, this is where the 2 smaller sorted arrays are
    //       'dst' is the destination, this is where you have to move data
    //       Merge the 2 smaller arrays using the same methodology as in 'Task1'
    int i=idx1,j=idx2;
    int current=idx1;
    // System.out.println(i +" "+ j+" "+end);
    while (i < idx2 && j < end){
      if(src[i] > src[j]){
          //System.out.println(src[i]+" "+src[j]);
          dst[current]=src[j];
          j++;
          //System.out.println(src[idx1]+" "+src[idx2]);
          //System.out.println();
          
      }
      else{
        dst[current]=src[i];
        i++;
      }
      

     //System.out.println(dst[current]);
      current++;
  }
  while(i < idx2){
    dst[current]=src[i];
    i++;
    //System.out.println(dst[current]);
    current++;
    
  }
  while(j < end){
    dst[current]=src[j];
    j++;
    //System.out.println(dst[current]);
    current++;
  }
    
    // System.out.println("Im merging :<");
    // for(var n:dst){
    //   System.out.println(n);
    // }
	}

  /* Recursive core, calls a sibling thread until max depth is reached */
  public static void kernel(int[] src, int[] dst, int start, int end, int depth) {

    /* Single thread sort if bottom has been reached*/
    // TODO: simply sort the array using 'Arrays.sort()' if depth is zero.
    if(depth == 0){
      Arrays.sort(dst,start,end);
    }


    /* Otherwise split task into two recursively */
    // TODO: summon another thread and recursively sort left and right half
    // NOTE: don't forget to make recursive call with 'depth-1'
    else{
      int pivot = (end-start)/2;
      //System.out.println(depth+" "+start+" "+(start+pivot)+" "+end);
      
      
      Thread t1 = new Thread(()->{
        kernel(dst,src,start+pivot,end,depth-1);
        
      });
      t1.start();
      
      
      kernel(dst,src,start,start+pivot,depth-1);
      
      try{
        
        t1.join();
        
      }
      
      catch(Exception ex){
        System.err.println(ex.getLocalizedMessage());
      }
      
      merge(src,dst,start,start+pivot,end);
      //System.out.println();
      //merge(src,dst,start,start+pivot,end);
      
    }
  }

  /* Creates a sorted version of any int array */
  public static int[] sort(int[] array) {

    /* Initialize variables */
    // TODO: Create 'src' and 'dst' arrays

    int[] src = new int[array.length], dst = new int[array.length];
    for(int i= 0; i< src.length;i++){
      src[i]=array[i];
      dst[i]=array[i];
    }
    /* Calculate optimal depth */
    int minSize   = 1000;
    int procNum   = Runtime.getRuntime().availableProcessors();
    int procDepth = (int) Math.ceil(Math.log(procNum) / Math.log(2));
    int arrDepth  = (int) (Math.log(array.length / minSize) / Math.log(2));
    int optDepth  = Math.max(0, Math.min(procDepth, arrDepth));
    //System.out.println(optDepth);
    /* Launch recursive core */
    // TODO: launch kernel, call with 'optDepth' (not 'optDepth-1')

    kernel(dst, src, 0, array.length, optDepth);
    //merge(src,dst,0,0+(array.length/2),array.length);

    //System.out.println(optDepth);
    // for(var n:src){
    //   System.out.println(n);
    // }
    // TODO: return src or dst depending on the parity of the used depth
    // TODO: delete all lines starting with '//'
    return src;

  }
}
