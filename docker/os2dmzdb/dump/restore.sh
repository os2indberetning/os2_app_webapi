find . -name '*.sql' | awk '{ print "source",$0 }' | mysql --batch -u root -p dmzdb