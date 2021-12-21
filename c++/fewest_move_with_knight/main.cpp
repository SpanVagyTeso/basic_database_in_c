//Fodor Kristóf RFXQHM

#include <iostream>
#include <queue>
#include <tuple>
#include <vector>

/*
 Példa bemenet
8 8
0 0
4 2 
*/

int max_x,max_y;
//Kiraktam a mozgást egy külön fv-be hogy olvashatóbb legyen a while ciklus 
void make_a_move_possibly(int x, int y,int d, std::vector<std::vector<int>>& table, std::queue<std::tuple<int,int>>& queue){
        //le ellenőrizzük, hogy a megadott koordináták rajta vannak e a pályán
        if(x<0 || x >= max_x || y < 0 || y >= max_y){
            return;
        }
        //ellenőrizzük hogy láttuk e már ezt a tile-t
        if(table[x][y]!=0)return ;

        //beállítjuk, hogy a tile milyen messze van a huszártól
        table[x][y]=d+1;
        queue.push(std::tuple<int,int>(x,y));
     
    }

int main(){

    int n=0,m=0;
    
    std::cout <<"Tabla merete:"<<std::endl; 
    std::cin >> n >> m;
    
    
    //inicializáljuk a táblát, hogy default minden 0-a távolságra van
    std::vector<std::vector<int>> table;
    table.resize(n);
    for(int i = 0;i < n ;i++){
        table[i].resize(m);
        for(int j=0;j<m;j++){
            table[i][j]=0;
        }
    }

    max_x = n;
    max_y = m;

    int start_x,start_y;
    int end_x,end_y;
    std::cout <<"Huszar kezdo helye:"<<std::endl;
    std::cin >> start_x>>start_y;
    std::cout <<"Huszar vege helye:"<<std::endl;
    std::cin >> end_x >> end_y;


    //ha a pálya 1 széles vagy magas akkor a huszár nem tud lépni szóval le kell kezelni ezt a külön esetet
    if(n <=1 || m <=1 ){
        if(start_x==end_x && start_y == end_y){
            //de ha a start és a vég ugyanott van technikailag van megoldás
            std::cout<<"0 lepes kell"<<std::endl;
            return 0;
        }
        std::cout<<"Nincs megoldas mert a huszarnak legalabb 2 szeles/magas palya kell"<<std::endl;
        return 0;
    }
    
    std::queue<std::tuple<int,int>> queue;
    queue.push(std::tuple<int,int>(start_x,start_y));

    bool solvable=false;

    //std::cout << queue.empty()<<std::endl;

    while(!queue.empty()){
        std::tuple<int,int> current = queue.front();
        queue.pop();
        int x,y;
        x= std::get<0>(current);
        y= std::get<1>(current);

        

        if(x == end_x && y == end_y){
            std::cout<<table[x][y]<<" lepes kell"<<std::endl;
            solvable=true;
            break;
        }

        int d = table[x][y];


        //Nem találtam hirtelen szebb megoldást :/ lehet lambda fv-kel meglehetett volna oldani egy picit szebben de nem hiszem hogy a huszár valamikor is fog máshogy lépni
        make_a_move_possibly(x+2,y-1,d,table,queue);
        
        make_a_move_possibly(x+2,y+1,d,table,queue);

        make_a_move_possibly(x-2,y-1,d,table,queue);
        
        make_a_move_possibly(x-2,y+1,d,table,queue);
        
        make_a_move_possibly(x-1,y+2,d, table,queue);

        make_a_move_possibly(x+1,y+2,d, table,queue);
        
        make_a_move_possibly(x-1,y-2,d, table,queue);

        make_a_move_possibly(x+1,y-2,d, table,queue);
        
        
    }
    
    if(!solvable){
        std::cout<<"Nincs megoldas"<<std::endl;
    }

}