import os
import datetime

commit_date =datetime.datetime.strptime('2018-05-22 9:11:21','%Y-%m-%d %H:%M:%S') 
now=datetime.datetime.strptime('2018-05-26 9:11:21','%Y-%m-%d %H:%M:%S')

while commit_date < now:
    os.system('git init ')
    os.system('git add .')
    os.system('git commit --date={time} -m "test lala" '.format(time=commit_date.isoformat()))
    os.system('git push -u HZJC0001L master')
    print(commit_date)
    commit_date=datetime.timedelta(days=1)
    print(commit_date)



