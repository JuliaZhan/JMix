import os
import datetime

start='2018-05-22'
end='2018-05-26'
tiemstart=datetime.datetime.strptime(start,'%Y-%m-%d')
timeend=datetime.datetime.strptime(end,'%Y-%m-%d')

while tiemstart < timeend:
    f=open('data.txt','a+')
    os.system('git init ')
    os.system('git add .')
    os.system('git commit --date={time} -m "test lala" '.format(time=tiemstart.isoformat()))
    os.system('git push -u HZJC0001L master')
    f.writelines(tiemstart.isoformat() + '\n')
    print(tiemstart)
    tiemstart+=datetime.timedelta(days=1)
    print(tiemstart)



