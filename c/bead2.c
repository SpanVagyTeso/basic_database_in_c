#include<stdio.h> 
#include<stdlib.h> 
#include<unistd.h> 
#include<sys/types.h> 
#include<string.h> 
#include<sys/wait.h> 
#include <string.h>

#define DAY1 6
#define DAY2 4
#define DAY3 2
#define DAY4 12
#define DAY5 3
#define DAY6 2
#define DAY7 7

int days[7];

int maxdays[7];

typedef struct employee{
    char name[60];
    char address[60];
    int days[7]; /* 0 nem tud 1 tud */
}employee;

char getWorkPlaceinChar(const char*);
char getJobinChar(const char*);
char* getWorkPlaceinStr(const char);
char* getJobinStr(const char);

void parentHandler(int sig);
void childHandler(int sig);
void childHandler2(int sig);

int childWhile=0;
int childWhile2=0;
int parentWhile=0;

int whichDayint(const char* day){
    if(strcmp(day,"hétfő")==0)return 0;
    if(strcmp(day,"kedd")==0)return 1;
    if(strcmp(day,"szerda")==0)return 2;
    if(strcmp(day,"csütörtök")==0)return 3;
    if(strcmp(day,"péntek")==0)return 4;
    if(strcmp(day,"szombat")==0)return 5;
    if(strcmp(day,"vasárnap")==0)return 6;
    return -1;
}

char* whichDaystring(int day){
    if(day==0)return "hétfő";
    if(day==1)return "kedd";
    if(day==2)return "szerda";
    if(day==3)return "csütörtök";
    if(day==4)return "péntek";
    if(day==5)return "szombar";
    if(day==6)return "vasárnap";
    
}

void read_in_console(const char *console_text, char *result){
    printf(console_text);
    scanf("\n%[^\t\n]s", result);
    char* r = result;
    if(strstr(result, ";") != NULL){
        result=NULL;
    }
}

void open_file(FILE *fp){
    fp = fopen("adatbazis.txt","r");
}

int readNextEmp(FILE *fp,employee* ret){
    char adat[300];
    char tempdays[300];

    if(fgets(adat, sizeof(adat), fp) == NULL)return 0;

    strcpy(ret->name,strtok(adat, ";"));

    strcpy(ret->address,strtok(NULL, ";"));

    strcpy(tempdays,strtok(NULL, ";"));
    
    sscanf(tempdays,"%d %d %d %d %d %d %d",&ret->days[0], &ret->days[1], &ret->days[2], &ret->days[3], &ret->days[4], &ret->days[5], &ret->days[6]);
    
    return 1;
}


void removeElement(){
    char cid[10];
    read_in_console("Adja meg annak az ID-jét akit kiszeretne rakni a rendszerből :\n",cid);
    
    int i=1;
    int id;
    sscanf(cid, "%d", &id);

    FILE* infile = fopen("adatbazis.txt","r");
    FILE* outfile = fopen("temp.txt","w");
    if (infile == NULL || outfile == NULL){
        fprintf(stderr,"BOOOOO ROSSZAK A FÁJLOK.");
        return;
    }
    
    employee emp;
    
    while(readNextEmp(infile,&emp)){
        
        if (i==id){
            ++i;
            continue;
        }
        
        fprintf(outfile,"%s;%s;",emp.name,emp.address);
        
        for(int i=0;i<6;i++){
            fprintf(outfile,"%d ", emp.days[i]);
        }
        
        fprintf(outfile,"%d;\n", emp.days[6]);

    }
    fclose(infile);
    fclose(outfile);
    
    remove("adatabazis.txt");
    rename("temp.txt", "adatbazis.txt");
}

void listArray(){
    int i=1;

    FILE* fp;
    fp=fopen("adatbazis.txt","r");
    if (fp == NULL){
        fprintf(stderr,"BOOOOO ROSSZAK A FÁJLOK.");
        return;
    }
    employee emp;
    while(readNextEmp(fp, &emp)){
        printf("\nId: %d \nName: %s \nAddress: %s \nDays: ",i, emp.name,emp.address);
        for(int j=0;j<7;j++){
            if(emp.days[j])printf("%s ",whichDaystring(j));
        }
        i++;
    }
    printf("\n\n");
    fclose(fp);
}

void listByDay(){
    char cday[10];
    read_in_console("Melyik napot szeretnéd kilistázni? \n",cday);
    printf("\n");

    int day=whichDayint(cday);

    if(day==-1)return;
    
    FILE* fp;
    fp=fopen("adatbazis.txt","r");
    
    employee emp;
    while(readNextEmp(fp,&emp)){
        if(emp.days[day]==1){
            printf("Name:%s\nAddress:%s\n",emp.name,emp.address);
        }
    }
    
    fclose(fp);

}




void addArray(char* name, char* address, int* tempdays){
    for(int i=0;i<7;i++){
        if(tempdays[i]==1){
            if(days[i] >= maxdays[i]){
                printf("\n\n\n%s nap tele van már!\n\n\n",whichDaystring(i));
                return;
            }
        }
    }
    FILE *fp;
    fp = fopen("adatbazis.txt", "a");
    fprintf(fp,"%s;%s;",name,address);
    for(int i=0;i<6;i++){
        fprintf(fp,"%d ", tempdays[i]);
        if(tempdays[i] == 1)days[i]++;
    }
    
    fprintf(fp,"%d;\n", tempdays[6]);
    fclose(fp);
}



int addArrayCommand(){
    char name[60];
    char address[60];
    int tempdays[7];
    char cdays[60];
    int good=1;

    read_in_console("Name:",name);
    read_in_console("Address:",address);
    read_in_console("Napok:",cdays);

    if (strcmp(cdays, "-\0") != 0)
    {
        for(int i=0;i<7;i++)tempdays[i]=0;
        
        char* napok=strtok(cdays, " ");
        
        if(whichDayint(napok)==-1)good=0;
        

        tempdays[whichDayint(napok)]=1;
        
        

        while(napok = strtok(NULL, " ")){
            if(whichDayint(napok)==-1)good=0;
            tempdays[whichDayint(napok)]=1;
            if(days[whichDayint(napok)]>=maxdays[whichDayint(napok)])good=0;
        }
        
        

    }
    else good = 0;
    
    if(good)addArray(name,address,tempdays);

}

void createDataBase(){
    FILE* fp = fopen("adatbazis.txt","r");
    
    maxdays[0]=DAY1;    
    maxdays[1]=DAY2;    
    maxdays[2]=DAY3;    
    maxdays[3]=DAY4;    
    maxdays[4]=DAY5;    
    maxdays[5]=DAY6;    
    maxdays[6]=DAY7;

    for(int i =0;i<7;i++){
        days[i]=0;
    }
    
    if(fp == NULL){
        
    }
    else{
        char adat[300];
        size_t nread;
        char ccdays[60];
        int cdays[7];
        while (fgets(adat, sizeof(adat), fp) != NULL){
            
            char* adatok=strtok(adat, ";");
            adatok= strtok(NULL,";");
            adatok = strtok(NULL,";");
            strcpy(ccdays,adatok);

            sscanf(ccdays,"%d %d %d %d %d %d %d",&cdays[0], &cdays[1], &cdays[2], &cdays[3], &cdays[4], &cdays[5], &cdays[6]);
            for(int i=0;i<7;i++){
                days[i]+=cdays[i];
            }
        }
        if (ferror(fp)) {
            /* deal with error */
        }
        fclose(fp);
        for(int i=0;i<7;i++){
            printf("%d",days[i]);
        }
    }
}

void modifyArrayElement(){
    char cid[10];
    read_in_console("Adja meg annak az ID-jét akinek az adatait szeretné megváltoztatni :\n",cid);
    int id;
    sscanf(cid, "%d", &id);
    char command;
    char adat[60];
    printf("Mit szeretne megváltoztatni? \n1 - Név \n2 - Cím\n");
    getchar();
    command = getchar();
    
    switch(command){
        case '1': 
            read_in_console("Adjon meg egy új nevet:\n",adat);
            break;
        case '2': 
            read_in_console("Adjon meg egy új címet:\n",adat);
            break;  
    }
 

    printf("%c",command);
    FILE *fp = fopen("adatbazis.txt","r");
    FILE* out = fopen("temp.txt","w");
    if (fp == NULL || out == NULL){
        fprintf(stderr,"BOOOOO ROSSZAK A FÁJLOK.");
        return;
    }
    employee emp;
    int i=1;

    while(readNextEmp(fp,&emp)){
        if(i==id){
            switch(command){
                case '1': 
                    fprintf(out,"%s;%s;%d %d %d %d %d %d %d;\n",
                    adat,emp.address,emp.days[0],emp.days[1],emp.days[2],emp.days[3],emp.days[4],emp.days[5],emp.days[6]);
                    break;
                case '2': 
                    fprintf(out,"%s;%s;%d %d %d %d %d %d %d;\n",
                    emp.name,adat,emp.days[0],emp.days[1],emp.days[2],emp.days[3],emp.days[4],emp.days[5],emp.days[6]);
                    break;  
            }   
        }
        else{
            fprintf(out,"%s;%s;%d %d %d %d %d %d %d;\n",
                    emp.name,emp.address,emp.days[0],emp.days[1],emp.days[2],emp.days[3],emp.days[4],emp.days[5],emp.days[6]);
        }
    }

    fclose(out);
    fclose(fp);
    
    remove("adatabazis.txt");
    rename("temp.txt", "adatbazis.txt");
}

void getRandomWorkers(){
    FILE* fp;
    fp=fopen("tempworkers.txt","w");
    char buffer[100];
    int good=1;
    while(good){
        read_in_console("Következő Random jelölt(Kilepeshez irjon be egy - jelet):\n",buffer);
        if(buffer[0]=='-')good=0;
        else{
            fprintf(fp,"%s\n",buffer);
        }
    }
    fclose(fp);
}

void doYourJob(){
    childWhile=0;
    parentWhile=0;
    
    signal(SIGUSR2,childHandler);
    signal(SIGUSR1,parentHandler);

    int pipe1[2],pipe2[2],pipe3[2],pipe4[2];
    pid_t parent;
    parent=getpid();
    if(pipe(pipe1)==-1){
        printf("Fail pipe1\n");
    }
    if(pipe(pipe2)==-1){
        printf("Fail pipe2\n");
    }
    if(pipe(pipe3)==-1){
        printf("Fail pipe3\n");
    }
    if(pipe(pipe4)==-1){
        printf("Fail pipe3\n");
    }
    pid_t firstchild =fork();
    
    if(firstchild == -1){ // Failure
        printf("I can't even make a child I'm a failure...\n");
    }
    if(firstchild == 0){ // FirstChild
        char line[100];

        read_in_console("Hol és mit kell dolgozni?\n",line);
        char workplace[100];
        char job[100];
        
        char dday[10];

        read_in_console("Milyen nap van ma?\n",dday);
        
        int iday=whichDayint(dday);

        char send[4];
        

        strcpy(workplace,strtok(line,","));
        strcpy(job,strtok(NULL,","));

        

        send[0]=getWorkPlaceinChar(workplace);
        send[1]=getJobinChar(job);
        send[2]='0'+iday;
        send[3]='\0';
    
        //printf("Workplace: %c \n",send[0]);
        //printf("Job: %c \n",send[1]);

        close(pipe1[0]);
        write(pipe1[1],send,4);


       // printf("idejutottam\n");
        kill(parent,SIGUSR1);
       // printf("idejutottam\n");
        exit(0);
    }
    else{
        
        pid_t secchild= fork();
        //printf("Itt vagyok1 %d %d %d\n",getpid(),parent,secchild);
        
        if(secchild == -1){
            printf("error");
        }
        if(secchild == 0){ // Second Child
            signal(SIGUSR1,childHandler);
            signal(SIGUSR2,childHandler2);
            //printf("Itt vagyok2\n");
            while(childWhile==0);

            close(pipe2[1]);
            char buffer[4];

            read(pipe2[0],buffer,3);
            close(pipe3[1]);

            int day = buffer[2]-'0';
            //printf("Day %d\n",day);

            kill(parent,SIGUSR1);
            int count=0;

            childWhile=0;
            childWhile2=0;
            printf("Emberek akik jonnek:\n");
            while(childWhile==0){
                //printf("WHILE\n");
                while(childWhile2==0);
                if(childWhile2==1){
                    count++;
                    char name[60];
                    read(pipe3[0],name,60);
                    printf("%s \n",name);
                    kill(parent,SIGUSR1);
                }
                else if(childWhile2==2){
                    break;
                }
                childWhile2=0;
            }
            if(count < maxdays[day]){
                FILE* fpp;
                fpp=fopen("tempworkers.txt","r");
                if(fpp != NULL){    
                    printf("Reggeli random munkasok:\n");
                    char line[60];
                    while (count < maxdays[day] && fgets(line, sizeof(line), fpp)!=NULL){
                        count++;
                        printf("%s",line);
                    }
                }
            }
            close(pipe4[0]);
            char str[10];

            sprintf(str, "%d", count);
            // Now str contains the integer as characters
            write(pipe4[1],str,10);
            kill(parent,SIGUSR1);
            //printf("btw a count ennyi: %s %d\n",str,count);
            
            //printf("Second Child megszunt\n");
            exit(0);
        }
        else{
            signal(SIGUSR1,parentHandler);
            while(parentWhile==0);
            parentWhile=0;
           // printf("Idejutott\n");
            close(pipe1[1]);

            char buffer[4];
            read(pipe1[0],buffer,4);

            char* wp = getWorkPlaceinStr(buffer[0]);
            char* j = getJobinStr(buffer[1]);
            char* d = whichDaystring(buffer[2]-'0');

            printf("Helyszin: %s\n",wp);
            printf("Munka: %s\n",j);
            printf("Nap: %s\n",d);

            close(pipe2[0]);
            close(pipe1[0]);
            close(pipe3[0]);

            write(pipe2[1],buffer,4);

            kill(secchild,SIGUSR1);
            
            while(parentWhile==0);
            
            FILE* fp;
            fp=fopen("adatbazis.txt","r");
            
            int day = buffer[2]-'0';
            employee emp;
            while(readNextEmp(fp,&emp)){
                if(emp.days[day]==1){
                    //printf("Name: %s \n",emp.name);
                    write(pipe3[1],emp.name,60);

                    parentWhile=0;
                    kill(secchild,SIGUSR2);
                    while(parentWhile==0);
                }
            }
            kill(secchild,SIGUSR1);
    
            fclose(fp);
            
            parentWhile=0;
            while(parentWhile==0);
            char numb[10];
            close(pipe4[1]);
            read(pipe4[0],numb,10);
            printf("Ennyi munkas kuldtunk ki: %s\n",numb);

            printf("\n");
        }
    }
}

int main(int argc, char** argv){
    int command;
    createDataBase();
    remove("tempworkers.txt");

    do
    {
        printf("-----MENU-----\n");
        printf("Dolgozo_felvetel - 1\n");
        printf("Lista - 2\n");
        printf("Napi lista - 3\n");
        printf("Törlés - 4\n");
        printf("Módosítás - 5\n");
        printf("Random dolgozók - 6\n");
        printf("Munka kiadása - 7\n");
        printf("Kilep - 8\n\n");
        
        switch (command)
        {
        case '1':
            addArrayCommand();
            break;
        case '8':
            printf("Kilepes\n");
            exit(0);
            break;
        case '2':
            listArray();
            break;
        case '3':
            listByDay();
            break;
        case '4':
            removeElement();
            break;
        case '5':
            modifyArrayElement();
            break;
        case '6':
            getRandomWorkers();
            break;
        case '7':
            doYourJob();
            break;
        default:
            break;
        }
    } while ((command = getchar()) != EOF);
    
}

char* getWorkPlaceinStr(const char c){
    if(c=='0')return "Jeno telek";
    if(c=='1')return "Lovas dulo";
    if(c=='2')return "Hosszu";
    if(c=='3')return "Selyem telek";
    if(c=='4')return "Malom telek";
    if(c=='5')return "Szula";
    else return "bo"; 
}

char* getJobinStr(const char c){
    if(c=='0')return "metszes";
    if(c=='1')return "rugyfakaszto permetezes";
    if(c=='2')return "tavaszi nyitas";
    if(c=='3')return "horolas";
    else return "bo"; 
}

char getJobinChar(const char* str){
    if(strcmp(str,"metszes")==0)return '0';
    if(strcmp(str,"rugyfakaszto permetezes")==0)return '1';
    if(strcmp(str,"tavaszi nyitas")==0)return '2';
    if(strcmp(str,"horolas")==0)return '3';
    else return '-';
}

char getWorkPlaceinChar(const char* str){
  //  printf("Kapott str %s\n",str);
    if(strcmp(str,"Jeno telek")==0)return '0';
    if(strcmp(str,"Lovas dulo")==0)return '1';
    if(strcmp(str,"Hosszu")==0)return '2';
    if(strcmp(str,"Selyem telek")==0)return '3';
    if(strcmp(str,"Malom telek")==0)return '4';
    if(strcmp(str,"Szula")==0)return '5';
    else return '-';

}

void parentHandler(int sig){
    parentWhile=1;
 //   printf("ParentHandler\n");
}
void childHandler(int sig){
    childWhile=1;
    childWhile2=2;
    //printf("ChildHandler\n");
}
void childHandler2(int sig){
    childWhile2=1;
    //printf("ChildHander2\n");
}


