
import java.util.Arrays;

/* Task1: slicing and merging on 1 thread, sorting slices is parralelized */
public class Task1 {

  /* Create new sorted array by merging 2 smaller sorted arrays */
  private static int[] merge(int[] arr1, int[] arr2) {
    int[] merged = new int[arr1.length+arr2.length];

    int i,j,k;
    i=0;
    j=0;
    k=0;
    while (i < arr1.length && j < arr2.length){
      if(arr1[i] < arr2[j]){
        merged[k]=arr1[i];
        i++;
        k++;
      }
      else{
        merged[k]=arr2[j];
        j++;
        k++;
      }
    }

    if(i != arr1.length){
      while(i < arr1.length){
        merged[k]=arr1[i];
        i++;
        k++;
      }
    }
    else{
      while(j <arr2.length){
        merged[k]=arr2[j];
        j++;
        k++;
      }

    }

    return merged;
  }

  /* Creates an array of arrays by slicing a bigger array into smaller chunks */
  public static int[][] slice(int[] arr, int k) {
    int[][] arr2d = new int[k][];
    int leftover = arr.length %k;
    int pieces = arr.length/k;
    for(int i = 0; i< arr2d.length;i++){
      if(leftover>0){
        arr2d[i] = new int[pieces+1];
        leftover--;
      }
      else{
        arr2d[i] = new int[pieces];
      }
    }
    int i = -1,j = 0;
    for(int n = 0; n<arr.length;n++){
      if(j==0){
        i++;
      }
      arr2d[i][j]=arr[n];
      j++;
      if(arr2d[i].length==j){
        j=0;
      }
    }


    return arr2d;
  }


  /* Creates a sorted version of any int array */
  public static int[] sort(int[] array) {

    /* Initialize variables */
    // TODO: check available processors and create necessary variables
    Runtime runtime = Runtime.getRuntime();
    int availableProcessors = runtime.availableProcessors();
    //System.out.println(availableProcessors);


    /* Turn initial array into array of smaller arrays */
    // TODO: use 'slice()' method to cut 'array' into smaller bits

    int[][] arr2d = slice(array,availableProcessors);

    

    /* parralelized sort on the smaller arrays */
    // TODO: use multiple threads to sort smaller arrays (Arrays.sort())

    Thread[] threads = new Thread[availableProcessors];
    for(int i= 0; i< availableProcessors;i++){
      int thread_index = i;
      threads[i] = new Thread(()->{
        Arrays.sort(arr2d[thread_index]);
      });
      threads[i].start();
    }
    for(var i : threads){
      try{
        i.join();
      }
      catch(Exception ex){
        System.err.println(ex.getMessage());
      }
    }

    int [] sorted = merge(arr2d[0],arr2d[1]);
    for(int i =2;i<availableProcessors;i++){
      sorted = merge(sorted,arr2d[i]);
    }


    /* Merge sorted smaller arrays into a singular larger one */
    // TODO: merge into one big array using 'merge()' multiple times
    //       create an empty array called 'sorted' and in a for cycle use
    //       'merge(sorted, arr2d[i])' where arr2d is an array of sorted arrays

    /* Return fully sorted array */
    // TODO: return the sorted array and delete all lines starting with '//'
    return sorted;
  }
}
