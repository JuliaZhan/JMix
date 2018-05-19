import os
import datetime

start='2018-05-20'
end='2018-05-27'
tiemstart=datetime.datetime.strptime(start,'%Y-%m-%d')
timeend=datetime.datetime.strptime(end,'%Y-%m-%d')

while tiemstart < timeend:
    os.system('git init ')
    os.system('git add .')
    os.system('git commit --date={time} -m "test lala" '.format(time=tiemstart.isoformat()))
    os.system('git push -u HZJC0001L master')
    print(tiemstart)
    tiemstart+=datetime.timedelta(days=1)
    print(tiemstart)



