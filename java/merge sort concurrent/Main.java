
public class Main {
    public static void main(String[] args){
        int[] hello1 ={2, 6, 8, 3 ,7 ,10};
        int idx1=0, idx2=3,len=hello1.length;
        int tmp;
        while (idx1 < len && idx2 < len){
            if(hello1[idx1] > hello1[idx2]){
                System.out.println(hello1[idx1]+" "+hello1[idx2]);
                tmp = hello1[idx1];
                hello1[idx1]=hello1[idx2];
                hello1[idx2]=tmp;
                System.out.println(hello1[idx1]+" "+hello1[idx2]);
                
            }
            idx1++;
            if(idx1==idx2){
                idx2++;
            }
        }

       
        for(var n: hello1){
            System.out.print(n+" ");
        }
        System.out.println();

    }
    
}
